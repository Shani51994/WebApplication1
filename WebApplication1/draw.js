function circle(lon, lat, myCanvas) {
    myCanvas.lineWidth = '3';
    myCanvas.strokeStyle = "navy";
    myCanvas.beginPath();
    myCanvas.arc(lon, lat, 5, 0, 2 * Math.PI);
    myCanvas.fillStyle = "red";
    myCanvas.fill();
    myCanvas.stroke();
}


function drawPath(allLocations, myCanvas) {
    circle(allLocations[0].x, allLocations[0].y, myCanvas);
    myCanvas.beginPath();
    myCanvas.moveTo(allLocations[0].x, allLocations[0].y);
    for (var i = 1; i < allLocations.length; i++) {
       myCanvas.lineTo(allLocations[i].x, allLocations[i].y);
        myCanvas.strokeStyle = "red";
        myCanvas.stroke();
    }
}

function emptyArray(allLocations) {
    while (allLocations.length > 0) {
        allLocations.pop();
    }
}