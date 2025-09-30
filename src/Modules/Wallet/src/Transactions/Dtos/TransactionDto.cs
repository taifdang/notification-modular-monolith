using Wallet.Transactions.Enums;

namespace Wallet.Transactions.Dtos;
public record TransactionDto(Guid Id, decimal Amount, TransactionType TransactionType, TransactionStatus TransactionStatus, DateTime? CreateAt);

