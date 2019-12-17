using Xamarin.Forms;

namespace Visib.Mobile.Controls
{
    public class CustomEditor : Editor
    {

        public CustomEditor()
        {
            TextChanged += (sender, e) => { this.InvalidateMeasure(); };

            MaxLength = 200;
        }
    }
}
