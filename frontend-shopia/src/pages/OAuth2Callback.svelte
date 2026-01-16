<script>
  import { callback } from '$services/oAuth2Service.js';
  import { setLogin } from '$services/loginService.js';
  import { pushNotification } from '$libs/notification.js';
  import { navigate } from '$libs/router';

  $effect(() => {
    let matches = /\/oauth2callback\/([^/]+)\/([^/]+)/.exec(location.pathname);
    if (!matches) {
      navigate('/login');
      return;
    }

    let name = matches[1];
    let action = matches[2];
    let search = window.location.search;
    let deviceToken = localStorage.getItem('deviceToken');
    if (deviceToken)
      search += (search ? '&' : '?') + `deviceToken=${deviceToken}`;

    callback(name, action, search)
      .then(res => {
        setLogin(res);
        const searchParams = new URLSearchParams(window.location.search);
        const state = searchParams.get('state');
        if (state
          && !state.startsWith('/login')
          && !state.startsWith('/oauth2callback')
        ) {
          navigate(state);
          return;
        }

        navigate('/');
      })
      .catch(() => {
        pushNotification('Error de autorización.', 'error');
        navigate('/login');
      });
  });

</script>

Ingresando...
