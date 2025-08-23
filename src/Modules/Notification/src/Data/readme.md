add-migration initial -Context NotificationDbContext -Project Notification -StartupProject Api -o Data\Migrations
update-database -Context NotificationDbContext

# email
{
    "subject": "{{subject}} - Order #{{entityId}}",
    "body": {
      "html": "<html><body><h1>Thank you for your order!</h1><p>Your order #{{entityId}} has been confirmed. .</p></body></html>",
      "plainText": "{{body}}"
    },
    "attachments": [
      {
        "url": "https://example.com/invoice{{entityId}}.pdf",
        "fileName": "invoice{{entityId}}.pdf",
        "mimeType": "application/pdf"
      }
    ]
 }

 # push
 {
      "title": "Order Confirmed",
      "body": "Your order #{{entityId}} has been confirmed. Check your email for details.",
      "icon": "https://example.com/icon.png",
      "action": 
      {
        "type": "viewOrder",
        "url": "https://example.com/order/{{entityId}}"
      }
 }

 # sms
 {
    "smsText": "Thank you for your order! Order #{{OrderId}} confirmed."
 }

 # genneral
 {
  "messageId": "msg12345",
  "channel": "email",
  "recipient": {
    "userId": "user789",
    "email": "user@example.com"
  },
  "messageContent": {
    "subject": "Order Confirmation - Order #123456",
    "body": {
      "html": "<p>Thank you for your order #123456!</p>",
      "plainText": "Thank you for your order #123456!"
    },
    "attachments": [
      {
        "url": "https://example.com/invoice123456.pdf",
        "fileName": "invoice123456.pdf"
      }
    ]
  },
  "metadata": {
    "priority": "high",
    "templateId": "orderConfirmationTemplate",
    "requestId": "req9876"
  }
}