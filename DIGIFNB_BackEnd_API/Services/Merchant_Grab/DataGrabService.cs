using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading.Tasks;
using System.Linq;
using SeleniumExtras.WaitHelpers;
using DIGIFNB_BackEnd_API.Models.Grab.Order;
using Microsoft.EntityFrameworkCore;
using DIGIFNB_BackEnd_API.Data;
using DIGIFNB_BackEnd_API.Models.Grab.Feedback;

namespace DIGIFNB_BackEnd_API.Services.Merchant_Grab
{
    public class DataGrabService : IDataGrabService
    {
        private static readonly Random _random = new Random();
        // Hàm tạo khoảng delay ngẫu nhiên từ 3500 đến 6000 ms
        private readonly ApplicationDBContext _context;
        private Task RandomDelay()
        {
            int delay = _random.Next(5500, 7000);
            return Task.Delay(delay);
        }
        public async Task<string> CrawlDataFeedBackGrabAsync(string username, string password)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // Chạy trình duyệt ở chế độ headless (ẩn)

            using (var driver = new ChromeDriver(options))
            {
                try
                {
                    // Mở trang đăng nhập
                    driver.Navigate().GoToUrl("https://merchant.grab.com/portal");
                    await Task.Delay(2000);
                    //RandomDelay();

                    // Chấp nhận cookie
                    var acceptCookieButton = driver.FindElement(By.XPath("//*[@id=\"onetrust-accept-btn-handler\"]"));
                    acceptCookieButton.Click();
                    //await Task.Delay(1000);
                    RandomDelay();

                    // Nhập username
                    var usernameField = driver.FindElement(By.XPath("//*[@id=\"username\"]"));
                    usernameField.SendKeys(username);
                    //await Task.Delay(2000);
                    RandomDelay();

                    // Nhập password
                    var passwordField = driver.FindElement(By.XPath("//*[@id=\"password\"]"));
                    passwordField.SendKeys(password);
                    //await Task.Delay(2000);
                    RandomDelay();

                    // Click vào nút đăng nhập
                    var loginButton = driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div/div[2]/div[2]/div/div[3]/form/div/button"));
                    loginButton.Click();
                    await Task.Delay(5500);
                    //RandomDelay();

                    // Chờ cho đến khi nút 'Next' có thể click được
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                    var nextButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[3]/div/div[2]/div/div[2]/div/div[2]/button[2]")));
                    nextButton.Click();
                    //await RandomDelay();

                    // Chờ cho đến khi nút 'Close' có thể click được và đóng cửa sổ popup
                    var closeButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[5]/div/div[2]/div/div[2]/div/div/div/div[3]/button[1]")));
                    closeButton.Click();
                    //await RandomDelay();

                    // Click vào feedback
                    var feedbackField = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"root\"]/div/div/div[1]/aside/div[1]/div[1]/ul/li[4]/div/span[2]")));
                    feedbackField.Click();
                    //await RandomDelay();

                    // Lấy dữ liệu feedback
                    var elemsCode = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector(".css-1obojd0-FromColumn"))).Select(e => e.Text).ToList();
                    var elemsName = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector(".css-1bnu0w9-FromColumn"))).Select(e => e.Text).ToList();
                    var elemsStore = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector(".css-exht9r-StoreBranchColumn"))).Select(e => e.Text).ToList();
                    var elemsDate = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector(".css-eqs2us"))).Select(e => e.Text).ToList();


                    // Kết hợp dữ liệu tương tự như việc tạo DataFrame trong Python
                    var feedbackText = new List<string>();
                    for (int i = 0; i < elemsCode.Count; i++)
                    {
                        feedbackText.Add($"Code: {elemsCode[i]}, Name: {elemsName[i]}, Store: {elemsStore[i]}, Date: {elemsDate[i]}");
                    }

                    // Trả về chuỗi ghép từ danh sách
                    return string.Join("||", feedbackText);

                    //var rows = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector(".dui-table-row")));

                    //var feedbackList = new List<Feedback>();

                    //foreach (var row in rows)
                    //{
                    //    // Lấy các giá trị từ các thẻ <td>
                    //    var cells = row.FindElements(By.CssSelector(".dui-table-cell"));

                    //    // Lấy rating
                    //    var ratingText = cells[0].FindElement(By.CssSelector(".O0ugAsq8d2cCiyDUhqsp")).Text;
                    //    int rating = int.Parse(ratingText);

                    //    // Lấy review
                    //    var review = cells[1].FindElement(By.CssSelector(".reply-content span span")).Text;

                    //    // Lấy tên khách hàng và CustomerID
                    //    var customer = cells[2].FindElement(By.CssSelector(".css-1bnu0w9-FromColumn")).Text;
                    //    var customerId = cells[2].FindElement(By.CssSelector(".css-1obojd0-FromColumn")).Text;

                    //    // Lấy store
                    //    var store = cells[3].FindElement(By.CssSelector(".css-exht9r-StoreBranchColumn")).Text;

                    //    // Lấy type (Public/Private)
                    //    var type = cells[4].FindElement(By.CssSelector(".css-1reqkxi")).Text;

                    //    // Lấy thời gian tạo
                    //    var createdAtText = cells[5].FindElement(By.CssSelector(".css-eqs2us")).Text;
                    //    var createdAt = DateTime.ParseExact(createdAtText, "dd MMM yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                    //    // Kiểm tra nếu Feedback đã tồn tại trong cơ sở dữ liệu
                    //    bool feedbackExists = await _context.Feedbacks.AnyAsync(f => f.CustomerID == customerId && f.CreatedAt == createdAt);
                    //    if (!feedbackExists)
                    //    {
                    //        // Tạo đối tượng Feedback mới
                    //        var feedback = new Feedback
                    //        {
                    //            Rating = rating,
                    //            Review = review,
                    //            Customer = customer,
                    //            CustomerID = customerId,
                    //            Store = store,
                    //            Type = type,
                    //            CreatedAt = createdAt
                    //        };

                    //        feedbackList.Add(feedback);  // Thêm vào danh sách
                    //    }
                    //}

                    //if (feedbackList.Count > 0)
                    //{
                    //    // Lưu feedback vào cơ sở dữ liệu
                    //    _context.Feedbacks.AddRange(feedbackList);
                    //    await _context.SaveChangesAsync();

                    //    return "New feedback data saved successfully.";
                    //}
                    //else
                    //{
                    //    return "No new feedback data to add.";
                    //}

                }
                catch (NoSuchElementException ex)
                {
                    return $"Error: Element not found - {ex.Message}";
                }
                catch (WebDriverTimeoutException ex)
                {
                    return $"Error: Timeout waiting for element - {ex.Message}";
                }
                finally
                {
                    driver.Quit(); // Đóng trình duyệt
                }
            }
        }
       public async Task<string> CrawlDataOrderGrabAsync(string username, string password)
        
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // Chạy trình duyệt ở chế độ headless (ẩn)

            using (var driver = new ChromeDriver(options))
            {
                try
                {
                    // Mở trang đăng nhập
                    driver.Navigate().GoToUrl("https://merchant.grab.com/portal");
                    await Task.Delay(2000);
                    //RandomDelay();

                    // Chấp nhận cookie
                    var acceptCookieButton = driver.FindElement(By.XPath("//*[@id=\"onetrust-accept-btn-handler\"]"));
                    acceptCookieButton.Click();
                    //await Task.Delay(1000);
                    RandomDelay();

                    // Nhập username
                    var usernameField = driver.FindElement(By.XPath("//*[@id=\"username\"]"));
                    usernameField.SendKeys(username);
                    //await Task.Delay(2000);
                    RandomDelay();

                    // Nhập password
                    var passwordField = driver.FindElement(By.XPath("//*[@id=\"password\"]"));
                    passwordField.SendKeys(password);
                    //await Task.Delay(2000);
                    RandomDelay();

                    // Click vào nút đăng nhập
                    var loginButton = driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div/div[2]/div[2]/div/div[3]/form/div/button"));
                    loginButton.Click();
                    await Task.Delay(5500);
                    //RandomDelay();

                    // Chờ cho đến khi nút 'Next' có thể click được
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                    var nextButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[3]/div/div[2]/div/div[2]/div/div[2]/button[2]")));
                    nextButton.Click();
                    //await RandomDelay();

                    // Chờ cho đến khi nút 'Close' có thể click được và đóng cửa sổ popup
                    var closeButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[5]/div/div[2]/div/div[2]/div/div/div/div[3]/button[1]")));
                    closeButton.Click();
                    //await RandomDelay();

                    // Click vào Order 
                    var OrderField = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"root\"]/div/div/div[1]/aside/div[1]/div[1]/ul/li[3]/div/span[2]")));
                    OrderField.Click();
                    //await RandomDelay();

                    // Click vào Order 
                    var HistoryField = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"rc-tabs-0-tab-history\"]")));
                    HistoryField.Click();
                    //await RandomDelay();

                    var rows = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector(".dui-table-row")));

                    var historyList = new List<History>();

                    foreach (var row in rows)
                    {
                        // Lấy các giá trị từ các thẻ <td>
                        var cells = row.FindElements(By.CssSelector(".dui-table-cell"));

                        var longOrderId = cells[1].Text;
                        if (string.IsNullOrEmpty(longOrderId))
                        {
                            // Nếu longOrderId là null hoặc rỗng, bỏ qua dòng này
                            continue;
                        }
                        var shortOrderId = cells[2].Text;
                        var totalAmountText = cells[3].Text.Replace("₫", "").Replace(".", "").Trim();
                        var totalAmount = decimal.Parse(totalAmountText);
                        var status = cells[4].FindElement(By.CssSelector(".dui-tag")).Text;

                        // Kiểm tra nếu LongOrderId đã tồn tại trong cơ sở dữ liệu
                        bool orderExists = await _context.Historys.AnyAsync(h => h.LongOrderId == longOrderId);
                        if (!orderExists)
                        {
                            // Tạo đối tượng History mới
                            var list = new History
                            {
                                LongOrderId = longOrderId,
                                ShortOrderId = shortOrderId,
                                TotalAmount = totalAmount,
                                Status = status
                            };

                            historyList.Add(list);  // Thêm vào danh sách
                        }
                    }

                    if (historyList.Count > 0)
                    {
                        // Lưu lịch sử vào database
                        _context.Historys.AddRange(historyList);
                        await _context.SaveChangesAsync();

                        return "New history data saved successfully.";
                        
                    }
                    else
                    {
                        return "No new history data to add.";
                    }
                }
                catch (NoSuchElementException ex)
                {
                    return $"Error: Element not found - {ex.Message}";
                    
                }
                catch (WebDriverTimeoutException ex)
                {
                    return $"Error: Timeout waiting for element - {ex.Message}";
                    
                }
                finally
                {
                    driver.Quit(); // Đóng trình duyệt
                }
            }
        }

        public async Task<List<History>> GetOrdersWithoutSavingAsync(string username, string password)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // Chạy trình duyệt ở chế độ headless (ẩn)

            using (var driver = new ChromeDriver(options))
            {
                try
                {
                    // Mở trang đăng nhập
                    driver.Navigate().GoToUrl("https://merchant.grab.com/portal");
                    await Task.Delay(2000);
                    //RandomDelay();

                    // Chấp nhận cookie
                    var acceptCookieButton = driver.FindElement(By.XPath("//*[@id=\"onetrust-accept-btn-handler\"]"));
                    acceptCookieButton.Click();
                    //await Task.Delay(1000);
                    RandomDelay();

                    // Nhập username
                    var usernameField = driver.FindElement(By.XPath("//*[@id=\"username\"]"));
                    usernameField.SendKeys(username);
                    //await Task.Delay(2000);
                    RandomDelay();

                    // Nhập password
                    var passwordField = driver.FindElement(By.XPath("//*[@id=\"password\"]"));
                    passwordField.SendKeys(password);
                    //await Task.Delay(2000);
                    RandomDelay();

                    // Click vào nút đăng nhập
                    var loginButton = driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div/div[2]/div[2]/div/div[3]/form/div/button"));
                    loginButton.Click();
                    await Task.Delay(5500);
                    //RandomDelay();

                    // Chờ cho đến khi nút 'Next' có thể click được
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                    var nextButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[3]/div/div[2]/div/div[2]/div/div[2]/button[2]")));
                    nextButton.Click();
                    //await RandomDelay();

                    // Chờ cho đến khi nút 'Close' có thể click được và đóng cửa sổ popup
                    var closeButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[5]/div/div[2]/div/div[2]/div/div/div/div[3]/button[1]")));
                    closeButton.Click();
                    //await RandomDelay();

                    // Click vào Order 
                    var OrderField = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"root\"]/div/div/div[1]/aside/div[1]/div[1]/ul/li[3]/div/span[2]")));
                    OrderField.Click();
                    //await RandomDelay();

                    // Click vào Order 
                    var HistoryField = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"rc-tabs-0-tab-history\"]")));
                    HistoryField.Click();
                    //await RandomDelay();

                    // Lấy dữ liệu các Order
                    var rows = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector(".dui-table-row")));

                    var historyList = new List<History>();

                    foreach (var row in rows)
                    {
                        // Lấy các giá trị từ các thẻ <td>
                        var cells = row.FindElements(By.CssSelector(".dui-table-cell"));

                        var longOrderId = cells[1].Text;
                        var shortOrderId = cells[2].Text;
                        var totalAmountText = cells[3].Text.Replace("₫", "").Replace(".", "").Trim();
                        var totalAmount = decimal.Parse(totalAmountText);
                        var status = cells[4].FindElement(By.CssSelector(".dui-tag")).Text;

                        // Tạo đối tượng History mới và thêm vào danh sách
                        var history = new History
                        {
                            LongOrderId = longOrderId,
                            ShortOrderId = shortOrderId,
                            TotalAmount = totalAmount,
                            Status = status
                        };

                        historyList.Add(history);
                    }

                    return historyList;  // Trả về danh sách Order
                }
                catch (NoSuchElementException ex)
                {
                    throw new Exception($"Error: Element not found - {ex.Message}");
                }
                catch (WebDriverTimeoutException ex)
                {
                    throw new Exception($"Error: Timeout waiting for element - {ex.Message}");
                }
                finally
                {
                    driver.Quit(); // Đóng trình duyệt
                }
            }
        }
    }
}
