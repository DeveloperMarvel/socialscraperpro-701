using HtmlAgilityPack;
using OpenQA.Selenium;
using ScraperCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ScraperCore
{
    public class SocialInfoScraper : ScraperBase
    {


        public void Search(string keyword)
        {
            this.Driver.Search(By.Name(GoogleSelector.SearchBoxName), keyword);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SocialModel> ExtractCurrentPageData(bool isEmail = false)
        {
            var rList = new List<SocialModel>();
            var pageHtml = this.Driver.PageSource;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(pageHtml);
            var list = htmlDoc.DocumentNode.SelectNodes(GoogleSelector.SearchList);
            foreach (var item in list)
            {
                var model = new SocialModel();
                model.HomePage = item.SelectSingleNode(GoogleSelector.ItemHome)?.Attributes["href"]?.Value;
                var nameList = ParseName(item.SelectSingleNode(GoogleSelector.ItemName)?.InnerText);
                model.Name = nameList[0];
                model.Position = nameList[1];
                model.Company = nameList[2];
                model.Address = item.SelectSingleNode(GoogleSelector.ItemAddress)?.InnerText;
                var description = item.SelectSingleNode(GoogleSelector.ItemDescrption)?.InnerText;
                if (string.IsNullOrEmpty(description))
                {
                    description = item.SelectSingleNode(GoogleSelector.ItemDescrption1)?.InnerText;
                }
                model.Email = description.ExtractEmail();
                model.Tel = description.ExtractTel();
                model.Description = description;
                if (isEmail)
                {
                    if (!string.IsNullOrEmpty(model.Email))
                    {
                        rList.Add(model);
                    }
                }
                else
                {
                    rList.Add(model);
                }

            }
            return rList;

        }
        public bool NextPage()
        {
            var nextBtn = this.Driver.FindElementWait(By.Id(GoogleSelector.PageNextBtnId));
            if (nextBtn.Displayed)
            {
                this.Driver.MoveToElementAndClick(nextBtn);
                return true;
            }
            return false;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalNum"></param>
        public void ExtractData(int totalNum, Action<List<SocialModel>> action, int delay = 5, CancellationToken cancel = default, bool isEmail = false)
        {
            var extactNum = 0;
            while (extactNum < totalNum)
            {
                SpinWait.SpinUntil(() => false, delay * 1200);
                try
                {
                    if (!this.IsExitsVerifyCode())
                    {
                        var list = this.ExtractCurrentPageData(isEmail);
                        extactNum += list.Count;
                        action(list);
                        if (extactNum == 0 || cancel.IsCancellationRequested)
                        {
                            break;
                        }
                        var isNext = this.NextPage();
                        if (!isNext)
                        {
                            break;
                        }
                    }
                    else
                    {

                    }

                }
                catch (WebDriverException ex)
                {
                    break;
                }
                catch (Exception ex)
                {
                    continue;
                }

            }
        }


        public bool IsExitsVerifyCode()
        {
            var elment = this.Driver.FindElementCatch(By.Id("recaptcha"));
            return elment.Displayed;
        }
        private List<string> ParseName(string sourceStr)
        {
            if (string.IsNullOrEmpty(sourceStr))
            {
                return new List<string> {
                    string.Empty,
                    string.Empty,
                    string.Empty };
            }
            var str = sourceStr.Split('|').FirstOrDefault();
            var list = str.Split('-').ToList();
            var num = list.Count;
            for (int i = 0; i < 3 - num; i++)
            {
                list.Add(string.Empty);
            }
            return list;
        }
    }
}
