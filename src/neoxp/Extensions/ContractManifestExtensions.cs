// Copyright (C) 2015-2023 The Neo Project.
//
// ContractManifestExtensions.cs file belongs to neo-express project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.SmartContract;
using Neo.SmartContract.Manifest;

namespace NeoExpress;

internal static class ContractManifestExtensions
{
    public static bool IsNep11Compliant(this ContractManifest manifest)
    {
        try
        {
            var symbolMethod = manifest.Abi.GetMethod("symbol", 0);
            var decimalsMethod = manifest.Abi.GetMethod("decimals", 0);
            var totalSupplyMethod = manifest.Abi.GetMethod("totalSupply", 0);
            var balanceOfMethod1 = manifest.Abi.GetMethod("balanceOf", 1);
            var balanceOfMethod2 = manifest.Abi.GetMethod("balanceOf", 2);
            var tokensOfMethod = manifest.Abi.GetMethod("tokensOf", 1);
            var ownerOfMethod = manifest.Abi.GetMethod("ownerOf", 1);
            var transferMethod1 = manifest.Abi.GetMethod("transfer", 3);
            var transferMethod2 = manifest.Abi.GetMethod("transfer", 5);

            var symbolValid = symbolMethod.Safe == true &&
                symbolMethod.ReturnType == ContractParameterType.String;
            var decimalsValid = decimalsMethod.Safe == true &&
                decimalsMethod.ReturnType == ContractParameterType.Integer;
            var totalSupplyValid = totalSupplyMethod.Safe == true &&
                totalSupplyMethod.ReturnType == ContractParameterType.Integer;
            var balanceOfValid1 = balanceOfMethod1.Safe == true &&
                balanceOfMethod1.ReturnType == ContractParameterType.Integer &&
                balanceOfMethod1.Parameters[0].Type == ContractParameterType.Hash160;
            var balanceOfValid2 = balanceOfMethod2?.Safe == true &&
                balanceOfMethod2?.ReturnType == ContractParameterType.Integer &&
                balanceOfMethod2?.Parameters[0].Type == ContractParameterType.Hash160 &&
                balanceOfMethod2?.Parameters[0].Type == ContractParameterType.ByteArray;
            var tokensOfValid = tokensOfMethod.Safe == true &&
                tokensOfMethod.ReturnType == ContractParameterType.InteropInterface &&
                tokensOfMethod.Parameters[0].Type == ContractParameterType.Hash160;
            var ownerOfValid1 = ownerOfMethod.Safe == true &&
                ownerOfMethod.ReturnType == ContractParameterType.Hash160 &&
                ownerOfMethod.Parameters[0].Type == ContractParameterType.ByteArray;
            var ownerOfValid2 = ownerOfMethod.Safe == true &&
                ownerOfMethod.ReturnType == ContractParameterType.InteropInterface &&
                ownerOfMethod.Parameters[0].Type == ContractParameterType.ByteArray;
            var transferValid1 = transferMethod1.Safe == false &&
                transferMethod1.ReturnType == ContractParameterType.Boolean &&
                transferMethod1.Parameters[0].Type == ContractParameterType.Hash160 &&
                transferMethod1.Parameters[1].Type == ContractParameterType.ByteArray &&
                transferMethod1.Parameters[2].Type == ContractParameterType.Any;
            var transferValid2 = transferMethod2?.Safe == false &&
                transferMethod2?.ReturnType == ContractParameterType.Boolean &&
                transferMethod2?.Parameters[0].Type == ContractParameterType.Hash160 &&
                transferMethod2?.Parameters[1].Type == ContractParameterType.Hash160 &&
                transferMethod2?.Parameters[2].Type == ContractParameterType.Integer &&
                transferMethod2?.Parameters[3].Type == ContractParameterType.ByteArray &&
                transferMethod2?.Parameters[4].Type == ContractParameterType.Any;
            var transferEvent = manifest.Abi.Events.SingleOrDefault(s =>
                s.Name == "transfer" &&
                s.Parameters.Length == 3 &&
                s.Parameters[0].Type == ContractParameterType.Hash160 &&
                s.Parameters[1].Type == ContractParameterType.Hash160 &&
                s.Parameters[2].Type == ContractParameterType.Integer &&
                s.Parameters[3].Type == ContractParameterType.ByteArray) != null;

            // Waiting on issue https://github.com/neo-project/neo-devpack-dotnet/issues/812
            // decimals is being set to safe for Nep11Token in the neo compiler

            return (symbolValid &&
                decimalsValid &&
                totalSupplyValid &&
                (balanceOfValid2 || balanceOfValid1) &&
                tokensOfValid &&
                (ownerOfValid2 || ownerOfValid1) &&
                (transferValid2 || transferValid1) &&
                transferEvent);
        }
        catch
        {
            return false;
        }
    }

    public static bool IsNep17Compliant(this ContractManifest manifest)
    {
        try
        {
            var symbolMethod = manifest.Abi.GetMethod("symbol", 0);
            var decimalsMethod = manifest.Abi.GetMethod("decimals", 0);
            var totalSupplyMethod = manifest.Abi.GetMethod("totalSupply", 0);
            var balanceOfMethod = manifest.Abi.GetMethod("balanceOf", 1);
            var transferMethod = manifest.Abi.GetMethod("transfer", 4);

            var symbolValid = symbolMethod.Safe == true &&
                symbolMethod.ReturnType == ContractParameterType.String;
            var decimalsValid = decimalsMethod.Safe == true &&
                decimalsMethod.ReturnType == ContractParameterType.Integer;
            var totalSupplyValid = totalSupplyMethod.Safe == true &&
                totalSupplyMethod.ReturnType == ContractParameterType.Integer;
            var balanceOfValid = balanceOfMethod.Safe == true &&
                balanceOfMethod.ReturnType == ContractParameterType.Integer &&
                balanceOfMethod.Parameters[0].Type == ContractParameterType.Hash160;
            var transferValid = transferMethod.Safe == false &&
                transferMethod.ReturnType == ContractParameterType.Boolean &&
                transferMethod.Parameters[0].Type == ContractParameterType.Hash160 &&
                transferMethod.Parameters[1].Type == ContractParameterType.Hash160 &&
                transferMethod.Parameters[2].Type == ContractParameterType.Integer &&
                transferMethod.Parameters[3].Type == ContractParameterType.Any;
            var transferEvent = manifest.Abi.Events.SingleOrDefault(s =>
                s.Name == "transfer" &&
                s.Parameters.Length == 3 &&
                s.Parameters[0].Type == ContractParameterType.Hash160 &&
                s.Parameters[1].Type == ContractParameterType.Hash160 &&
                s.Parameters[2].Type == ContractParameterType.Integer) != null;

            return (symbolValid &&
                decimalsValid &&
                totalSupplyValid &&
                balanceOfValid &&
                transferValid &&
                transferEvent);
        }
        catch
        {
            return false;
        }
    }
}
