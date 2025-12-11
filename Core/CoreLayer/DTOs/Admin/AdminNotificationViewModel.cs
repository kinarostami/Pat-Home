namespace CoreLayer.DTOs.Admin
{
    public class AdminNotificationViewModel
    {
        public int MessageCount { get; set; }
        public int NotificationCount { get; set; }
        public string OrderNotifications { get; set; }
        public string TicketNotifications { get; set; }
        public string MessageNotifications { get; set; }
        public string WithdrawalNotifications { get; set; }
        public string NewSellerNotifications { get; set; }
        public string ProductCommentsNotifications { get; set; }
        public string ArticleCommentsNotifications { get; set; }
        public string NotificationResult
        {
            get
            {
                var res = "";
                if (OrderNotifications != null)
                {
                    res += OrderNotifications;
                }
             
                if (TicketNotifications != null)
                {
                    res += TicketNotifications;
                }
                if (ArticleCommentsNotifications != null)
                {
                    res += ArticleCommentsNotifications;
                }
                if (ProductCommentsNotifications != null)
                {
                    res += ProductCommentsNotifications;
                }
                if (WithdrawalNotifications != null)
                {
                    res += WithdrawalNotifications;
                }
                if (NewSellerNotifications != null)
                {
                    res += NewSellerNotifications;
                }
                return res;
            }
        }
    }
}