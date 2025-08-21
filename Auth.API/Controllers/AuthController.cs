[HttpPost("revoke")]
[Authorize]
public async Task<IActionResult> RevokeCurrentToken(CancellationToken cancellationToken)
{
    var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
    var exp = User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;

    if (string.IsNullOrEmpty(jti) || string.IsNullOrEmpty(exp))
    {
        return BadRequest("Invalid token claims");
    }

    var expiry = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp)).DateTime;
    await _revocationService.RevokeTokenAsync(jti, expiry, cancellationToken);

    return Ok(new { message = "Token revoked successfully" });
}

[HttpPost("revoke-all")]
[Authorize]
public async Task<IActionResult> RevokeAllTokens(CancellationToken cancellationToken)
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(userId))
    {
        return BadRequest("Invalid user");
    }

    await _revocationService.RevokeAllUserTokensAsync(userId, cancellationToken);
    return Ok(new { message = "All tokens revoked successfully" });
}
