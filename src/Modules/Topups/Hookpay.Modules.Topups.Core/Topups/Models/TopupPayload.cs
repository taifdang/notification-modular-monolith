using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Topups.Core.Topups.Models;

public record TopupPayload(
        string gateway,
        string transactionDate,
        string? accountNumber,
        string? subAccount,
        string? code,
        string? content,
        string transferType,
        string description,
        int transferAmount,
        string? referenceCode,
        decimal accumulated,
        int id);

