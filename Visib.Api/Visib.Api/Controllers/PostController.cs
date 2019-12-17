using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Visib.Api.Data;
using Visib.Api.Models;
using Visib.Api.ViewModels;

namespace Visib.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public PostController(UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _mapper = mapper;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Authorize]
        [Route("share/{postId:guid}")]
        public async Task<IActionResult> Share(Guid postId)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var post = await _dbContext.Posts.SingleOrDefaultAsync(n => n.Id == postId);
            if (post == null)
            {
                return new NotFoundResult();
            }
            var postCopy = new Post
            {
                Id = Guid.NewGuid(),
                AppUserId = user.Id,
                Description = post.Description,
                Title = post.Title,
                Tags = post.Tags,
                PublishDate = DateTime.Now,
                CategoryId = post.CategoryId
            };
            await _dbContext.Posts.AddAsync(postCopy);
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        [HttpPost]
        [Authorize]
        [Route("search/paged/{pageNumber}")]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetSearchPaged([FromBody]SearchViewModel request, int pageNumber)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var query = _dbContext.Posts.Include(n => n.AppUser).Include(n => n.Category).Include(n => n.PostLikes).AsQueryable();

            query = query.Where(n => n.Title.Contains(request.Term, StringComparison.InvariantCultureIgnoreCase) ||
            n.AppUser.Name.Contains(request.Term, StringComparison.InvariantCultureIgnoreCase) ||
            n.Tags.Contains(request.Term, StringComparison.InvariantCultureIgnoreCase) ||
            n.Category.Name.Contains(request.Term, StringComparison.InvariantCultureIgnoreCase));
            var posts = await query.Skip((pageNumber-1) * 5).Take(5).ToListAsync();

            return new OkObjectResult(posts.Select(post => new PostViewModel
            {
                Id = post.Id,
                Tags = post.Tags,
                Description = post.Description,
                Title = post.Title,
                CommentCount = post.CommentCount,
                IsLikedByUser = post.PostLikes.Any(n => n.AppUserId == user.Id),
                LikeCount = post.LikeCount,
                ViewCount = post.ViewCount,
                UserId = post.AppUser.Id,
                Username = post.AppUser.Name,
                PublishDate = post.PublishDate,
                CategoryId = post.CategoryId,
                CategoryName = post.Category.Name
            }));
        }

        [HttpGet]
        [Authorize]
        [Route("paged/{pageNumber}/{filterType}")]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetPaged(int pageNumber, FilterType filterType)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var query = _dbContext.Posts.Include(n => n.Category).Include(n => n.AppUser).Include(n => n.PostLikes).AsQueryable();
            switch (filterType)
            {
                case FilterType.Recent:
                    query = query.OrderByDescending(n => n.PublishDate);
                    break;
                case FilterType.Viewed:
                    query = query.OrderByDescending(n => n.ViewCount);
                    break;
                case FilterType.Applauded:
                    query = query.OrderByDescending(n => n.LikeCount);
                    break;
                default:
                    query = query.OrderByDescending(n => n.PublishDate);
                    break;
            }

            var posts = await query.Skip((pageNumber - 1) * 5).Take(5).ToListAsync();

            return new OkObjectResult(posts.Select(post => new PostViewModel
            {
                Id = post.Id,
                Tags = post.Tags,
                Description = post.Description,
                CommentCount = post.CommentCount,
                Title = post.Title,
                ViewCount = post.ViewCount,
                IsLikedByUser = post.PostLikes.Any(n => n.AppUserId == user.Id),
                LikeCount = post.LikeCount,
                UserId = post.AppUser.Id,
                Username = post.AppUser.Name,
                PublishDate = post.PublishDate,
                CategoryId = post.CategoryId,
                CategoryName = post.Category.Name
            }));
        }



        [HttpGet]
        [Authorize]
        [Route("user/{userId}")]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetPostsForUser(string userId)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(n => n.Id == userId);
            var query = _dbContext.Posts.Include(n => n.Category).Include(n => n.AppUser).Include(n => n.PostLikes).AsQueryable();
            query = query.Where(n => n.AppUserId == user.Id);
            var posts = await query
                .OrderByDescending(n => n.PublishDate).ToListAsync();

            return new OkObjectResult(posts.Select(post => new PostViewModel
            {
                Id = post.Id,
                Tags = post.Tags,
                Description = post.Description,
                Title = post.Title,
                ViewCount = post.ViewCount,
                CommentCount = post.CommentCount,
                IsLikedByUser = post.PostLikes.Any(n => n.AppUserId == user.Id),
                LikeCount = post.LikeCount,
                UserId = post.AppUser.Id,
                Username = post.AppUser.Name,
                PublishDate = post.PublishDate,
                CategoryId = post.CategoryId,
                CategoryName = post.Category.Name
            }));
        }

        [HttpGet]
        [Authorize]
        [Route("tags")]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetTags()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var query = await _dbContext.Posts.Select(n => n.Tags).ToListAsync();
            var tags = query.SelectMany(n => n.Split(' ')).Distinct().OrderBy(n => n);
            return new OkObjectResult(tags);
        }

        [HttpGet]
        [Authorize]
        [Route("comment/{postId:guid}")]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetComments(Guid postId)
        {
            var comments = await _dbContext.PostComments.Include(n => n.AppUser).Where(n => n.PostId == postId)
                .OrderByDescending(n => n.PublishDate).ToListAsync();

            return new OkObjectResult(comments.Select(comment => new PostCommentViewModel
            {
                Id = comment.Id,
                Description = comment.Description,
                PublishDate = comment.PublishDate,
                AppUserId = comment.AppUserId,
                PostId = comment.PostId,
                AppUsername = comment.AppUser.Name
            }));
        }

        [Authorize]
        [HttpPost]
        [Route("comment")]
        public async Task<ActionResult> PostComment([FromBody] PostCommentViewModel request)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var post = await _dbContext.Posts.SingleOrDefaultAsync(n => n.Id == request.PostId);

            if (post == null) return new NotFoundResult();

            var comment = new PostComment
            {
                Id = Guid.NewGuid(),
                AppUserId = user.Id,
                Description = request.Description,
                PublishDate = DateTime.Now,
                PostId = request.PostId
            };
            await _dbContext.PostComments.AddAsync(comment);
            post.CommentCount++;
            await _dbContext.SaveChangesAsync();
            request.Id = comment.Id;
            return new OkObjectResult(request);
        }

        [Authorize]
        [HttpPost]
        [Route("comment/{id:guid}")]
        public async Task<ActionResult> DeleteComment(Guid id)
        {
            var postComment = await _dbContext.PostComments.SingleOrDefaultAsync(n => n.Id == id);
            var post = await _dbContext.Posts.SingleOrDefaultAsync(n => n.Id == postComment.PostId);
            if (postComment == null) return new NotFoundResult();

            _dbContext.PostComments.Remove(postComment);
            post.CommentCount--;
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PostViewModel request)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var post = new Post
            {
                Id = request.Id,
                AppUserId = user.Id,
                Description = request.Description,
                Title = request.Title,
                Tags = request.Tags,
                PublishDate = DateTime.Now,
                CategoryId = request.CategoryId,
            };
            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();
            return new OkObjectResult(request);
        }

        [Authorize]
        [HttpPost]
        [Route("view/{id:guid}")]
        public async Task<ActionResult> View(Guid id)
        {
            var post = await _dbContext.Posts.SingleOrDefaultAsync(n => n.Id == id);

            post.ViewCount++;
            _dbContext.Update(post);
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        [Authorize]
        [HttpGet]
        [Route("like")]
        public async Task<ActionResult> GetLikes()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var postLikes = await _dbContext.PostLikes.Include(n=>n.AppUser).Include(n=>n.Post).Include(n=>n.Post).Where(n => n.Post.AppUserId == user.Id).ToListAsync();

            return new OkObjectResult(postLikes.Select(n=> new LikeViewModel
            { 
                PostId = n.PostId,
                PostTitle = n.Post.Title,
                Username = n.AppUser.Name,
                AppUserId = n.AppUserId,
                When = n.When
            }));
        }

        [Authorize]
        [HttpPost]
        [Route("like/{id:guid}")]
        public async Task<ActionResult> Like(Guid id)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var post = await _dbContext.Posts.SingleOrDefaultAsync(n => n.Id == id);

            if (post == null) return new NotFoundResult();

            var currentLike = _dbContext.PostLikes.SingleOrDefault(n => n.AppUserId == user.Id && n.PostId == id);

            if (currentLike == null)
            {
                if (post.PostLikes == null)
                {
                    post.PostLikes = new List<PostLike>();
                }
                post.LikeCount = post.LikeCount + 1;
                post.PostLikes.Add(new PostLike
                {
                    AppUserId = user.Id,
                    PostId = post.Id,
                    When = DateTime.Now
                });
            }
            else
            {
                post.LikeCount = post.LikeCount -= 1;
                post.PostLikes.Remove(currentLike);
            }
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }
    }

    public static class QueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");
        private static readonly FieldInfo QueryModelGeneratorField = typeof(QueryCompiler).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryModelGenerator");
        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");
        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        public static string ToSql<TEntity>(this IQueryable<TEntity> query)
        {
            var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var queryModelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = queryModelGenerator.ParseQuery(query.Expression);
            var database = DataBaseField.GetValue(queryCompiler);
            var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var sql = modelVisitor.Queries.First().ToString();

            return sql;
        }
    }
}