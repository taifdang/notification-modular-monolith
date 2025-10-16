add-migration initial -Context NotificationDbContext -Project Notification -StartupProject Api -o Infrastructure\Data\Migrations
update-database -Context NotificationDbContext
