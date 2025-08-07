add-migration initial -Context TopupDbContext -Project Topup -StartupProject Api -o Data\Migrations
update-database -Context TopupDbContext