using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using VkNet.Abstractions.Utils;
using VkNet.Exception;
using VkNet.Model;

namespace VkNet.Infrastructure.Authorization.ImplicitFlow.Forms;

/// <inheritdoc />
[UsedImplicitly]
public sealed class TwoFactorForm : AbstractAuthorizationForm
{
	/// <inheritdoc />
	public TwoFactorForm(IRestClient restClient, IAuthorizationFormHtmlParser htmlParser)
		: base(restClient, htmlParser)
	{
	}

	/// <inheritdoc />
	public override ImplicitFlowPageType GetPageType() => ImplicitFlowPageType.TwoFactor;

	/// <inheritdoc />
	protected override async Task FillFormFieldsAsync(VkHtmlFormResult form, IApiAuthParams authParams, CancellationToken token = default)
	{
		if (authParams.TwoFactorAuthorization == null && authParams.TwoFactorAuthorizationAsync == null)
		{
			throw new VkAuthorizationException("Двухфакторная авторизация должна быть установлена в " + nameof(IApiAuthParams));
		}

		if (authParams.TwoFactorAuthorization != null && form.Fields.ContainsKey(AuthorizationFormFields.Code))
		{
			form.Fields[AuthorizationFormFields.Code] = authParams.TwoFactorAuthorization.Invoke();
		}

		if (authParams.TwoFactorAuthorizationAsync != null && form.Fields.ContainsKey(AuthorizationFormFields.Code))
		{
			form.Fields[AuthorizationFormFields.Code] = await authParams.TwoFactorAuthorizationAsync;
		}
	}

}