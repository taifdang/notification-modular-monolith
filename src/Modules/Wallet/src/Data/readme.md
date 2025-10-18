add-migration InitialWalletDb -Context WalletDbContext -Project Wallet -StartupProject Api -o Data\Migrations\Wallet
update-database -Context WalletDbContext

add-migration InitialTopupSagaDb -Context TopupSagaDbContext -Project Wallet -StartupProject Api -o Data\Migrations\TopupSaga
update-database -Context TopupSagaDbContext -Project Wallet -StartupProject Api

