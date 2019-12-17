import { Component, OnInit } from '@angular/core';
import { ApiService } from '@app/core/services/api.service';
import { NotificationService } from '@app/core/services';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse } from '@angular/common/http';

@Component({
    selector: 'sa-backgroundVideo',
    templateUrl: './backgroundVideo.component.html',
})
export class BackgroundVideoComponent implements OnInit {

    public progress: number;
    public message: string;

    constructor(private http: HttpClient, private notificationService: NotificationService) {

    }

    ngOnInit() {

    }

    upload(files) {
        if (files.length === 0)
            return;

        const formData = new FormData();

        for (let file of files)
            formData.append('backgroundvideo.mp4', file);

        const uploadReq = new HttpRequest('POST', `https://admin.visib.com/api/BackgroundMedia`, formData, {
        //const uploadReq = new HttpRequest('POST', `https://localhost:44338/api/BackgroundMedia`, formData, {
            reportProgress: true,
        });

        this.http.request(uploadReq).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress)
                this.progress = Math.round(100 * event.loaded / event.total);
            else if (event.type === HttpEventType.Response)
                this.message = event.body.toString();
        });
    }

}
