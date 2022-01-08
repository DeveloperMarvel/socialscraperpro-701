namespace ScraperCore
{
    public class AmazonSelector
    {
        public const string SearchBox = "twotabsearchtextbox";
        public const string Select = "searchDropdownBox";
        public const string SearchBtn = "nav-search-submit-button";
    }

    public class GoogleSelector
    {
        public const string SearchBoxName = "q";
        public const string SearchBtnName = "btnK";

        public const string SearchList = "//div[@class='g']/div";
        public const string ItemHome = ".//a";
        public const string ItemName = "..//h3";
        public const string ItemAvatar = ".//img";
        public const string ItemAddress = ".//div[@class='MUxGbd wuQ4Ob WZ8Tjf']";
        public const string ItemDescrption = ".//div[@class='VwiC3b yXK7lf MUxGbd yDYNvb lyLwlc lEBKkf']";
        public const string ItemDescrption1 = ".//span[@class='aCOpRe ljeAnf']";

        public const string PageNum = "//div[role='navigation']//a[class='fl']";
        public const string PageNextBtnId = "pnnext";

        public const string Recaptcha = "//div[@id='recaptcha']";
    }
}
