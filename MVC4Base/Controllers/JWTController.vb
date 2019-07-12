Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports Microsoft.IdentityModel.Tokens
Imports System.IdentityModel.Tokens.Jwt
Imports System.Security.Claims

Public Class JWTController
    Inherits System.Web.Http.ApiController

    Private Shared sec As String = ConfigurationManager.AppSettings.Get("sec")
    Private Shared host As String = ConfigurationManager.AppSettings.Get("host")


    Shared Function JWTToken(ByVal name As String) As String
        Dim issuedAt As DateTime = DateTime.Now
        Dim expires As DateTime = DateTime.UtcNow.AddDays(7)
        Dim tokenHandler As JwtSecurityTokenHandler = New JwtSecurityTokenHandler()
        Dim claimsIdentity As ClaimsIdentity = New ClaimsIdentity({New Claim(ClaimTypes.Name, name)})

        Dim now = DateTime.UtcNow
        Dim securityKey = New Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec))
        Dim signingCredentials = New Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
        Dim token = CType(tokenHandler.CreateJwtSecurityToken(issuer:=host, audience:=host, subject:=claimsIdentity, notBefore:=issuedAt, expires:=expires, signingCredentials:=signingCredentials), JwtSecurityToken)
        Dim tokenStr As String = tokenHandler.WriteToken(token)
        Return tokenStr
    End Function

    Shared Function JWTUser(ByVal request As System.Net.Http.HttpRequestMessage) As String
        Dim headers = Request.Headers
        Dim tokenHandler As JwtSecurityTokenHandler = New JwtSecurityTokenHandler()

        If headers.Contains("Authorization") Then
            Dim tokenStr As String = headers.GetValues("Authorization").First
            Try
                Dim now = DateTime.UtcNow
                Dim securityKey = New Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec))
                Dim validationParameters As TokenValidationParameters = New TokenValidationParameters With {
                    .ValidIssuer = host,
                    .ValidAudiences = {host},
                    .ValidateLifetime = True,
                    .IssuerSigningKey = securityKey
                }
                Dim securityToken As SecurityToken
                Dim claims = tokenHandler.ValidateToken(tokenStr, validationParameters, securityToken)
                Return claims.Identity.Name
            Catch ex As ArgumentException
                Throw New HttpResponseException(HttpStatusCode.BadRequest)
            Catch ex As SecurityTokenExpiredException
                Throw New HttpResponseException(HttpStatusCode.Unauthorized)
            Catch ex As SecurityTokenDecryptionFailedException
                Throw New HttpResponseException(HttpStatusCode.Unauthorized)
            Catch ex As Exception
                Throw New HttpResponseException(HttpStatusCode.InternalServerError)
            End Try
        End If
        Throw New HttpResponseException(HttpStatusCode.BadRequest)
    End Function

    Shared Function JWTUser2(ByVal request As System.Web.HttpRequestBase) As String
        Dim headers = request.Headers
        Dim tokenHandler As JwtSecurityTokenHandler = New JwtSecurityTokenHandler()

        If headers.AllKeys.Contains("Authorization") Then
            Dim tokenStr As String = headers.GetValues("Authorization").First
            Try
                Dim now = DateTime.UtcNow
                Dim securityKey = New Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec))
                Dim validationParameters As TokenValidationParameters = New TokenValidationParameters With {
                        .ValidIssuer = host,
                        .ValidAudiences = {host},
                        .ValidateLifetime = True,
                        .IssuerSigningKey = securityKey
                        }
                Dim securityToken As SecurityToken
                Dim claims = tokenHandler.ValidateToken(tokenStr, validationParameters, securityToken)
                Return claims.Identity.Name
            Catch ex As ArgumentException
                Throw New HttpResponseException(HttpStatusCode.BadRequest)
            Catch ex As SecurityTokenExpiredException
                Throw New HttpResponseException(HttpStatusCode.Unauthorized)
            Catch ex As SecurityTokenDecryptionFailedException
                Throw New HttpResponseException(HttpStatusCode.Unauthorized)
            Catch ex As Exception
                Throw New HttpResponseException(HttpStatusCode.InternalServerError)
            End Try
        End If
        Throw New HttpResponseException(HttpStatusCode.BadRequest)
    End Function
End Class
