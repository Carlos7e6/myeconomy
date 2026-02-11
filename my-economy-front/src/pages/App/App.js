import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { supabase } from '../../components/supabaseClient/supabaseClient';
import Menu from "../../components/menu/menu";
import Savings from "../savings/Savings";
import Main from "../main/main";
import FixedCost from '../fixedCost/FixedCost';
import Auth from '../auth/Auth'; // Tu nuevo componente de Google Login

import "./App.css";

function App() {
  const [session, setSession] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // 1. Revisar sesión al cargar
    supabase.auth.getSession().then(({ data: { session } }) => {
      setSession(session);
      setLoading(false);
    });

    // 2. Escuchar cambios
    const { data: { subscription } } = supabase.auth.onAuthStateChange((_event, session) => {
      setSession(session);
    });

    return () => subscription.unsubscribe();
  }, []);

  if (loading) return <div>Cargando...</div>;

  return (
    <Router>
      <div className="App">
        {/* Si NO hay sesión, mostramos SOLO el login */}
        {!session ? (
          <Routes>
            <Route path="*" element={<Auth />} />
          </Routes>
        ) : (
          /* Si HAY sesión, mostramos el Menú y todas las rutas */
          <>
            <Menu />
            <Routes>
              <Route path="/" element={<Navigate to="/main" />} />
              <Route path="/savings" element={<Savings />} />
              <Route path="/main" element={<Main />} />
              <Route path="/fixedCost" element={<FixedCost session={session} />} />
              {/* Si intenta ir a una ruta que no existe, lo mandamos a main */}
              <Route path="*" element={<Navigate to="/main" />} />
            </Routes>
          </>
        )}
      </div>
    </Router>
  );
}

export default App;