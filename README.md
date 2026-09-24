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

# Upgrading to 1.5

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
