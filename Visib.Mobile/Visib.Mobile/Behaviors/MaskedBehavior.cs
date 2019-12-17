using System.Collections.Generic;
using Xamarin.Forms;

namespace Visib.Mobile.Behaviors
{
    public class MaskedBehavior : Behavior<Entry>
    {
        private string _mask = "";

        private IDictionary<int, char> _positions;

        public string Mask
        {
            get => _mask;
            set
            {
                _mask = value;
                SetPositions();
            }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void SetPositions()
        {
            if (string.IsNullOrEmpty(Mask))
            {
                _positions = null;
                return;
            }

            var list = new Dictionary<int, char>();
            for (var i = 0; i < Mask.Length; i++)
                if (Mask[i] != 'X')
                    list.Add(i, Mask[i]);

            _positions = list;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var entry = sender as Entry;
            var text = entry.Text;

            if (Mask == "XXX.XXX.XXX-XX" && text.Length > "XXX.XXX.XXX-XX".Length)
            {
                Mask = "XX.XXX.XXX/XXXX-XX";
                text = text.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
                SetPositions();
            }
            else if (Mask == "XX.XXX.XXX/XXXX-XX" && text.Length <= 14)
            {
                Mask = "XXX.XXX.XXX-XX";
                text = text.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
                SetPositions();
            }

            if (Mask == "(XX)XXXX-XXXX" && text.Length > "(XX)XXXX-XXXX".Length)
            {
                Mask = "(XX)XXXXX-XXXX";
                text = text.Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty);
                SetPositions();
            }
            else if (Mask == "(XX)XXXXX-XXXX" && text.Length <= 13)
            {
                Mask = "(XX)XXXX-XXXX";
                text = text.Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty);
                SetPositions();
            }






            if (string.IsNullOrWhiteSpace(text) || _positions == null)
                return;

            if (text.Length > _mask.Length)
            {
                entry.Text = text.Remove(text.Length - 1);
                return;
            }

            foreach (var position in _positions)
                if (text.Length >= position.Key + 1)
                {
                    var value = position.Value.ToString();
                    if (text.Substring(position.Key, 1) != value)
                        text = text.Insert(position.Key, value);
                }

            if (entry.Text != text)
                entry.Text = text;
        }
    }
}
