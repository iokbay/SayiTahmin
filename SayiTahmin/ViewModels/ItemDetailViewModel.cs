using System;

namespace SayiTahmin
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Guess Item { get; set; }
        public ItemDetailViewModel(Guess item = null)
        {
            Number = item?.number;
            ValueString = item?.valueString;
            Item = item;
        }
    }
}
