namespace Wallet.Wallets.Dtos;
public record WalletDto (Guid Id, Guid UserId, string PaymentCode, decimal Balance);

