/*
 * draw the circle on the canvs in the point(lon, lat)
 */
function circle(lon, lat, myCanvas) {
    myCanvas.lineWidth = '3';
    myCanvas.strokeStyle = "navy";
    myCanvas.beginPath();
    myCanvas.arc(lon, lat, 5, 0, 2 * Math.PI);
    myCanvas.fillStyle = "red";
    myCanvas.fill();
    myCanvas.stroke();
}


/*
 * the function get all points in the path, draw the first point and then the lines between the others 
 */
function drawPath(allLocations, myCanvas) {
    circle(allLocations[0].x, allLocations[0].y, myCanvas);
    myCanvas.beginPath();
    myCanvas.moveTo(allLocations[0].x, allLocations[0].y);
    for (var i = 1; i < allLocations.length; i++) {
        myCanvas.lineTo(allLocations[i].x, allLocations[i].y);
        myCanvas.lineWidth = '2';
        myCanvas.strokeStyle = "red";
        myCanvas.stroke();
    }
}
