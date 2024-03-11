window.addEventListener('DOMContentLoaded', (event) =>{
    getVisitCount();
})


const functionApi = 'https://httptriggercounterbs.azurewebsites.net/api/httptriggercounterbs?code=xowlwimrW8Wu6cmtVnXRM6YWLyi0BY8vri4bN86li85fAzFuD58lcA%3D%3D';
const localfunctionApi = 'http://localhost:7071/api/HttpTriggerCounterBS';


const getVisitCount = () =>{
    let count = 30;
    fetch(functionApi).then(response => {
        return response.json()
    }).then(response => {
        console.log("Website called function API.")
        count = response.count;
        document.getElementById("counter").innerText = count;
    }).catch(function(error){
        console.log(error);
    });
    return count
}