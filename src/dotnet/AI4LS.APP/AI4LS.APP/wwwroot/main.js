function initModals() {
    var elems = document.querySelectorAll('.modal');
    var instances = M.Modal.init(elems, {});
}
function initSelect() {
    var elems = document.querySelectorAll('select');
    var instances = M.FormSelect.init(elems, {});
}

//Ask lucas
function ScrolltoBottonListView(id) {
    var objDiv = document.getElementById(id);
    objDiv.scrollTop = objDiv.scrollHeight;   

} 



//Lucas 2018
function freezeLucasPanels() {
    document.getElementById("lucasLoader").classList.remove('active');
    document.getElementById("lucasLoader").classList.add('active');
    document.getElementById("lucasOptionsRow").style.visibility = "hidden";
    document.getElementById("plotsContainer").style.visibility = "hidden";
}
function unfreezeLucasPanels() {
    document.getElementById("lucasLoader").classList.remove('active');    
    document.getElementById("lucasOptionsRow").style.visibility = "visible";
    document.getElementById("plotsContainer").style.visibility = "visible";
}

