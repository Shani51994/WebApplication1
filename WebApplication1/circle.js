function circle(lontitude, latitude, myCanvas){
    myCanvas.lineWidth = '5';
    myCanvas.strokeStyle = "navy";
    myCanvas.beginPath();
    var lat = (latitude + 90) * (htmlCanvas.height / 180);
    var lon = (lontitude + 180) * (htmlCanvas.width / 360);
    myCanvas.arc(lon, lat, 7, 0, 2 * Math.PI);
    myCanvas.fillStyle = "red";
    myCanvas.fill();
    myCanvas.stroke();
}


function drawPath(startLon, startLat, allLocations, myCanvas) {
    //myCanvas.beginPath();
    //myCanvas.moveTo(startLon, startLat);
    for (var i = 0; i < allLocations.length; i++) {
       // myCanvas.lineTo(allLocations[i].x, allLocations[i].y);
            myCanvas.beginPath();
            myCanvas.arc(allLocations[i].x, allLocations[i].y, 7, 0, 2 * Math.PI);
            myCanvas.fillStyle = "red";
            myCanvas.fill();
            myCanvas.stroke();
    }
    //myCanvas.strokeStyle="navy";
    //myCanvas.stroke();
}