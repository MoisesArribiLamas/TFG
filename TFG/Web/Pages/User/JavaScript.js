
    function mueveReloj(){
        let momentoActual = new Date();
        let hh = momentoActual.getHours();
        let mm = momentoActual.getMinutes();
        let ss = momentoActual.getSeconds();  

        hh = (hh < 10) ? "0" + hh : hh;
        mm = (mm < 10) ? "0" + mm : mm;
        ss = (ss < 10) ? "0" + ss : ss;



        let time = hh + " : " + mm + " : " + ss;
        let reloj = document.querySelector('#reloj');
        reloj.innerHTML = time;
    
}

setInterval(mueveReloj, 1000);


//    function time(){
//        let momentoActual = new Date();
//        let hh = momentoActual.getHours();
//        let mm = momentoActual.getMinutes();
//        let ss = momentoActual.getSeconds();

//        hh = (hh < 10) ? "0" + hh : hh;
//        mm = (mm < 10) ? "0" + mm : mm;
//        ss = (ss < 10) ? "0" + ss : ss;



//        var digitaltime = hh + " : " + mm + " : " + ss;
//        document.getElementById("myclock").innerHTML = digitaltime;

//        setInterval(time, 1000);
//}

//time();