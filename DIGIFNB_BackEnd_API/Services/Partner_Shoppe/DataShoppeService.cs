using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace DIGIFNB_BackEnd_API.Services.Partner_Shoppe
{
    public class DataShoppeService : IDataShoppeService
    {
        private static readonly Random _random = new Random();
        private IWebDriver _driver;
        private WebDriverWait _wait;
        // Hàm tạo khoảng delay ngẫu nhiên từ 3500 đến 6000 ms
        private Task RandomDelay()
        {
            int delay = _random.Next(5500, 7000);
            return Task.Delay(delay);
        }
        public async Task<string> StartLoginAsync(string username, string password)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");

            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));

            try
            {
                // Mở trang đăng nhập
                _driver.Navigate().GoToUrl("https://partner.shopee.vn/");
                await RandomDelay();

                // Nhập username
                var usernameField = _driver.FindElement(By.XPath("//*[@id=\"app\"]/div/div/div/div/div[1]/form/div/div/div[1]/div/input"));
                usernameField.SendKeys(username);
                await RandomDelay();

                // Nhập password
                var passwordField = _driver.FindElement(By.XPath("//*[@id=\"app\"]/div/div/div/div/div[1]/form/div/div/div[2]/div/input"));
                passwordField.SendKeys(password);
                await RandomDelay();

                // Click vào nút đăng nhập
                var loginButton = _driver.FindElement(By.XPath("//*[@id=\"app\"]/div/div/div/div/div[1]/form/div/div/button"));
                loginButton.Click();
                await RandomDelay();

                // Chờ trang yêu cầu nhập mã OTP xuất hiện
                _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"app\"]/div/div/form/div/div[2]/div/input")));

                // Trả về thông báo để người dùng nhập OTP
                return "OTP required. Please call the VerifyOtp endpoint with your OTP code.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> VerifyOtpAndCrawlDataAsync(string otpCode)
        {
            if (_driver == null || _wait == null)
            {
                return "Session not started.";
            }

            try
            {
                // Nhập mã OTP
                var otpInput = _driver.FindElement(By.XPath("//*[@id=\"app\"]/div/div/form/div/div[2]/div/input"));
                otpInput.SendKeys(otpCode);
                await RandomDelay();

                // Click vào nút xác nhận OTP
                var confirmOtpButton = _driver.FindElement(By.XPath("//*[@id=\"app\"]/div/div/form/div/button"));
                confirmOtpButton.Click();
                await RandomDelay();

                // Tiếp tục các thao tác sau khi xác thực OTP
                var feedbackField = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"app\"]/div/div/div[2]/button")));
                feedbackField.Click();
                await RandomDelay();

                var elemsStore = _wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector(".merchantInfo"))).Select(e => e.Text).ToList();
                // Trả về kết quả
                return string.Join("\n", elemsStore);

                // sau khi trả về danh sách cửa hàng 
                // bây giờ tạo list để người dùng chọn 



                // sau đó 
                // Tiếp tục vào quản lý đơn hàng 
                var order_field = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"layout-container\"]/aside/div/ul/li[8]/div/span")));
                order_field.Click();
                await RandomDelay();

                // vào doanh thu
                var doanhthu_field = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"rc-menu-uuid-17744-2-/order-popup\"]/li[1]/span/a")));
                doanhthu_field.Click();

                //vào danh sách order 
                var listorder_field = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"root\"]/div/div[1]/section/div[2]/div[1]/table/tbody/tr/td[3]/div[2]/button")));
                listorder_field.Click();

                //lấy danh sách order
                var elemsList = _wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector(".page-table-row"))).Select(e => e.Text).ToList();
                // Trả về kết quả
                return string.Join("\n", elemsList);

            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
            finally
            {
                _driver?.Quit();
            }
        }
    }
}
