$(document).ready(setup1);

function go_to_music(){
  console.log("go_to_music")
  window.location = "https://open.spotify.com/playlist/4o6xGhaqZEn0D330z8E9kc";
}
function go_to_capHub(){
  console.log("go_to_capHub")
  window.location = "https://open.spotify.com/playlist/4o6xGhaqZEn0D330z8E9kc";
}
function go_to_myFridge(){
  console.log("go_to_myFridge")
  window.location = "https://open.spotify.com";
}
function setup1(){
  $('#music').click(go_to_music);
  $('#caphub').click(go_to_capHub);
  $('#fridge').click(go_to_myFridge);
}
