import { supabase } from '../../components/supabaseClient/supabaseClient'; // Asegúrate de que la ruta sea correcta

function Auth() {
  const loginWithGoogle = async () => {
    const { error } = await supabase.auth.signInWithOAuth({
      provider: 'google',
      options: {
        // A dónde volverá el usuario tras loguearse en Google
        redirectTo: window.location.origin 
      }
    });

    if (error) console.error("Error al conectar con Google:", error.message);
  };

  return (
    <div style={{ textAlign: 'center', marginTop: '50px' }}>
      <h1>Bienvenido a MyEconomy</h1>
      <button 
        onClick={loginWithGoogle}
        style={{ padding: '10px 20px', cursor: 'pointer', backgroundColor: '#4285F4', color: 'white', border: 'none', borderRadius: '5px' }}
      >
        Entrar con Google
      </button>
    </div>
  );
}

export default Auth;