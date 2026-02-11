import { useState } from "react";
import { Link } from "react-router-dom";
import { FaArrowAltCircleRight, FaArrowAltCircleLeft, FaAmazonPay } from "react-icons/fa";
import { GrMoney } from "react-icons/gr";
import { IoHome, IoLogOut } from "react-icons/io5"; // Importamos IoLogOut
import { supabase } from "../../components/supabaseClient/supabaseClient"; // Asegúrate de que la ruta a tu cliente es correcta
import React from "react";
import "./menu.css";

function Menu() {
    const maxWidth = "10vw"; 
    const minWidth = "10vw";
    const [dynamicWidth, setDynamicWidth] = useState(minWidth);
    
    const handleLogout = async () => {
        const { error } = await supabase.auth.signOut();
        if (error) console.error("Error al cerrar sesión:", error.message);
    };

    const handleClick = () => {
        setDynamicWidth(dynamicWidth === minWidth ? maxWidth : minWidth);
    };

    return (
        <div className="Menu" style={{ width: dynamicWidth }}>
            {/* Secciones de navegación */}
            <Link className="icon" to="/main"><IoHome/></Link>
            <Link className="icon" to="/savings"><GrMoney/></Link>
            <Link className="icon" to="/fixedCost"><FaAmazonPay/></Link>

            {/* Botón de Logout al final */}
            <div className="logout-section" style={{ marginTop: 'auto', marginBottom: '20px' }}>
                <button 
                    className="icon logout-btn" 
                    onClick={handleLogout}
                    style={{ background: 'none', border: 'none', cursor: 'pointer', color: 'inherit' }}
                >
                    <IoLogOut title="Cerrar Sesión" />
                </button>
            </div>
        </div> 
    );
}

export default Menu;