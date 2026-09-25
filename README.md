# EppLib.NET 

[![build and publish](https://github.com/CodeMakerInc/EppLib.NET/actions/workflows/build-and-publish.yml/badge.svg?branch=master)](https://github.com/CodeMakerInc/EppLib.NET/actions/workflows/build-and-publish.yml)

EppLib.NET is a .NET library implementing the Extensible Provisioning Protocol (EPP)

HOW TO: https://github.com/ademar/EppLib.NET/wiki

EPP (defined in RFC 5730) is a widely adopted protocol used to comunicate between a domain registrar and the different domain name registries* to provision and manage domain names, host names and contact details. 

EppLib.NET provides a library that makes easy for registrars to interact with registries implementing the EPP protocol.

Our library is a complete implementation of the EPP specification. Have a look at the How to section for code examples and recipes.

* We now include EPP extensions for CIRA (the .CA registry), Nominet (the .UK registry) and IIS (the .SE registry).

# NuGet

```bash
PM> Install-Package EppLib
```

# Transfers

`DomainTransfer` and `ContactTransfer` support every transfer operation in RFC 5731 and RFC 5733: `Request` (the default), `Query`, `Approve`, `Reject` and `Cancel`. Put the object's authorization code in `Password`.

Request a domain transfer, adding a year to the registration:

```csharp
var request = new DomainTransfer("example.com")
{
    Period = new DomainPeriod(1, "y"),
    Password = "2fooBAR"
};
var response = service.Execute(request);

var transfer = response.DomainTransferResult;
// transfer.TransferStatus is "pending" until the losing registrar or the registry acts.
```

Check on it later, or approve, reject or cancel it, with the same command and a different operation:

```csharp
var query = new DomainTransfer("example.com") { Operation = TransferOperation.Query, Password = "2fooBAR" };
var status = service.Execute(query).DomainTransferResult;

Console.WriteLine($"{status.TransferStatus}: requested by {status.RequestClientId} on {status.RequestDate}, " +
                  $"action due from {status.ActionClientId} by {status.ActionDate}");

// As the losing registrar:
service.Execute(new DomainTransfer("example.com") { Operation = TransferOperation.Approve });
```

When the authorization code belongs to one of the domain's contacts rather than the domain itself, set `AuthInfoRoid` to that contact's repository ID (the `roid` attribute on `pw`).

Contacts transfer the same way:

```csharp
var contactTransfer = service.Execute(new ContactTransfer("sh8013") { Password = "2fooBAR" });
// contactTransfer.ContactId, TransferStatus, RequestClientId, RequestDate, ActionClientId, ActionDate
```

Registries that need extra data use their own subclass, such as `CiraDomainTransfer` for .ca, which accepts the same `Operation`, `Period` and `Password`.

# Upgrading

Releases 1.4.1 to 1.7.0 change behavior you may depend on. Newest first:

## 1.7: complete DNSSEC (secDNS) support

1.7.0 implements the rest of RFC 5910: DNSKEY data (`SecDNSKeyData`), removing all DNSSEC data, changing `maxSigLife`, urgent updates, and reading DNSSEC data from info responses with `SecDNSInfData.FromResponse`.

Two changes to check:

- `SecDNSData.KeyTag` is now an `int` instead of a `short`, because key tags go up to 65535. Code that assigns a key tag still compiles, but you must rebuild your application: a binary built against an earlier version fails when it touches `KeyTag`. Code that reads `KeyTag` into a `short` needs a cast or an `int`.
- `SecDNSData` now has a `DigestType` property. Earlier versions always sent SHA-1 (`1`), and the default stays SHA-1 so existing code sends the same request. Most registries expect SHA-256 today, so set it explicitly:

```csharp
var extension = new SecDNSCreate();
extension.DsData.Add(new SecDNSData
{
    KeyTag = 54321,
    Algorithm = SecDNSAlgorithm.ECDSAP256SHA256,
    DigestType = SecDNSDigestType.SHA256,
    Digest = "E2D3C916F6DEEAC73294E8268FB5885044A833FC5459588F4A9184CFC41A5766"
});
domainCreate.Extensions.Add(extension);
```

## 1.6: dates are returned in UTC

EPP requires every date-time to be UTC (RFC 5731 §2.4, RFC 5732 §2.4, RFC 5733 §2.7). Earlier versions converted parsed dates to the local time of the machine running your code. From 1.6.0 these properties hold UTC values with `DateTimeKind.Utc`:

- `DomainRenewResponse.ExDate`
- Nominet `DataQuality.DateCommenced` and `DataQuality.DateToSuspend`
- Nominet `AbuseNotification.Date`
- Nominet `DomainsSuspendedNotification.CancelDate`

Values the registry sends without a zone designator (some Nominet dates) are taken as UTC. Before, they came back with `DateTimeKind.Unspecified` and the same clock time.

If your code shows these values to people, or compares them with `DateTime.Now`, update it:

```csharp
var expires = renewResponse.ExDate.Value;            // UTC
var expiresLocal = expires.ToLocalTime();            // for display in local time
var expired = expires < DateTime.UtcNow;             // compare against UtcNow, not Now
```

Dates exposed as strings, such as `Domain.ExDate` or `PollResponse.QDate`, are unchanged: they hold the registry's text as sent.

1.6.0 also fixes `DomainRenew` when given a full date-time. You can pass an `exDate` straight from an info response (for example `2026-04-03T22:00:00.0Z`), and `curExpDate` is now `2026-04-03` in every timezone. Before, machines east of UTC sent the next day, and the registry rejected the renew.

## 1.5: server certificates are validated

Starting with 1.5.0, `TcpTransport` validates the registry's server certificate: it must chain to a trusted root, match the host name and not be expired. Earlier versions accepted any certificate, which let a man-in-the-middle read your EPP login credentials.

Production registries use valid certificates, so no change is needed there. If you connect to a test (OT&E) environment that uses a self-signed certificate, `Connect()` will now fail with an `AuthenticationException`. Allow that one certificate by pinning its SHA-256 fingerprint:

```csharp
var transport = new TcpTransport("epp.test.example", 700, clientCertificate)
{
    // Test environments only. Accepts a valid certificate, or this exact self-signed one.
    ServerCertificateValidationCallback = (sender, certificate, chain, errors) =>
        errors == SslPolicyErrors.None ||
        certificate?.GetCertHashString(HashAlgorithmName.SHA256) == "PASTE_SHA256_FINGERPRINT_HERE"
};
```

To get the fingerprint (drop the colons from the output):

```bash
openssl s_client -connect epp.test.example:700 </dev/null 2>/dev/null | openssl x509 -noout -fingerprint -sha256
```

`GetCertHashString(HashAlgorithmName)` needs .NET Core 3.0 or later. On .NET Framework, compare `certificate.GetCertHashString()` against the SHA-1 fingerprint (`-sha1` in the command above) instead.

Don't return `true` unconditionally, and never set this callback when connecting to a production registry.

## 1.4.1: TLS version chosen by the operating system

`Connect()` used to default to TLS 1.0 when a client certificate was set, and servers that only accept TLS 1.2 or later reset the connection. From 1.4.1 the default is `SslProtocols.None`, so the operating system negotiates the best version it supports.

C# compiles default parameter values into the calling code, so rebuild your application against 1.4.1 or later to pick up the new default. If you pass `SslProtocols.Tls` to `Connect()` explicitly, remove the argument.
