document.getElementById('BonsaiNamesButton').addEventListener('click', function(){
    
    fetch('/files/bonsai.txt')
        .then(response => response.text())
        .then(bonsaiText => {

            // console.log('Bonsai Names Text:', bonsaiText);
            
            var bonsaiNames = bonsaiText.split('\n').map(name => name.trim());

            // console.log('Bonsai Names List:', bonsaiText);
            bonsaiNamesSuffle = bonsaiNames.sort(() => Math.random() - 0.5);

            var teamListHead = document.querySelectorAll('.team-heading');

            // console.log('Team List Headings:', teamListHead);

            teamListHead.forEach((heading, index) => {
                if (index < bonsaiNames.length){
                    heading.textContent = bonsaiNames[index];
                }
                
            });
        })
        .catch(error => console.error('Error fetching bonsai names:', error));
});

$(document).ready(function (){
    $('#CreateTeams').click(function (event){
        var names = $('#names').val();
        var regex = /^[a-zA-Z\s,.\-_'']+$/;
        if(!names.trim()){
            alert("Names are required.");
            event.preventDefault();
            return;
        }
        if (!regex.test(names)){
            alert("Names can only contain letters, spaces, and the characters ,.-_'");
            event.preventDefault();
        }
        var teamSize = $('#teamSize').val();
        if(teamSize < 2 || teamSize >10){
            alert("Team size must only be between 2 and 10");
            event.preventDefault();
        }
    });
});
