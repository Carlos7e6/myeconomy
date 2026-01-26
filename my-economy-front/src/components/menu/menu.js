import { useState } from "react";
import { FaArrowAltCircleRight,FaArrowAltCircleLeft, FaAmazonPay } from "react-icons/fa";
import { GrMoney } from "react-icons/gr";
import { IoHome } from "react-icons/io5";
import React from "react";
import "./menu.css";

function Menu() {
    const maxWith = "10vw";
    const minWith = "10vw";
    const [dynamicWidth, setDynamicWidth] = useState(minWith);
    
  const handleClick = () => {
    if (dynamicWidth === minWith) {
      setDynamicWidth(maxWith);
    } else {
      setDynamicWidth(minWith);
    }
  };

  return (
    <div className="Menu" style={{ width: dynamicWidth }}>
        {/* {dynamicWidth === maxWith ? 
        (<FaArrowAltCircleLeft className="hamb icon" onClick={handleClick}></FaArrowAltCircleLeft>)
        : 
        (<FaArrowAltCircleRight className="hamb icon" onClick={handleClick}></FaArrowAltCircleRight>)} */}
        <a className="icon" href="/main"><IoHome/></a>
        <a className="icon" href="/savings"><GrMoney/></a>
        <a className="icon" href="/fixedCost"><FaAmazonPay/></a>
    </div>
  );
}

export default Menu;
