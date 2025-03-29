// Scripts personnalisés pour LogCentralPlatform

// Attendre que le DOM soit chargé
document.addEventListener('DOMContentLoaded', function() {
    // Activer tous les tooltips Bootstrap
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function(tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Activer tous les popovers Bootstrap
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function(popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // Ajouter la classe active au lien de navigation actuel
    var currentPath = window.location.pathname;
    var navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    
    navLinks.forEach(function(link) {
        var href = link.getAttribute('href');
        if (href && currentPath.indexOf(href) !== -1) {
            link.classList.add('active');
        }
    });

    // Toggle pour afficher/masquer les détails des exceptions
    var toggleExceptionBtns = document.querySelectorAll('.toggle-exception');
    
    toggleExceptionBtns.forEach(function(btn) {
        btn.addEventListener('click', function(e) {
            e.preventDefault();
            var targetId = this.getAttribute('data-target');
            var targetElement = document.getElementById(targetId);
            
            if (targetElement) {
                if (targetElement.classList.contains('d-none')) {
                    targetElement.classList.remove('d-none');
                    this.innerHTML = '<i class="bi bi-chevron-up"></i> Masquer les détails';
                } else {
                    targetElement.classList.add('d-none');
                    this.innerHTML = '<i class="bi bi-chevron-down"></i> Afficher les détails';
                }
            }
        });
    });
    
    // Animation d'entrée pour les cartes du tableau de bord
    var dashboardCards = document.querySelectorAll('.dashboard-widget');
    if (dashboardCards.length > 0) {
        setTimeout(function() {
            dashboardCards.forEach(function(card, index) {
                setTimeout(function() {
                    card.classList.add('show');
                }, index * 100);
            });
        }, 200);
    }

    // Configuration de DateRangePicker pour les filtres de date si la bibliothèque est disponible
    if (typeof $.fn.daterangepicker !== 'undefined') {
        $('.date-range-picker').daterangepicker({
            locale: {
                format: 'DD/MM/YYYY',
                applyLabel: 'Appliquer',
                cancelLabel: 'Annuler',
                fromLabel: 'Du',
                toLabel: 'Au',
                customRangeLabel: 'Période personnalisée',
                daysOfWeek: ['Di', 'Lu', 'Ma', 'Me', 'Je', 'Ve', 'Sa'],
                monthNames: ['Janvier', 'Février', 'Mars', 'Avril', 'Mai', 'Juin', 'Juillet', 'Août', 'Septembre', 'Octobre', 'Novembre', 'Décembre'],
                firstDay: 1
            },
            ranges: {
               'Aujourd\'hui': [moment(), moment()],
               'Hier': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
               '7 derniers jours': [moment().subtract(6, 'days'), moment()],
               '30 derniers jours': [moment().subtract(29, 'days'), moment()],
               'Ce mois': [moment().startOf('month'), moment().endOf('month')],
               'Mois précédent': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            },
            autoApply: true,
            opens: 'left'
        });
    }
});

// Fonction pour copier du texte dans le presse-papiers
function copyToClipboard(text, btnElement) {
    navigator.clipboard.writeText(text).then(function() {
        var originalText = btnElement.innerHTML;
        btnElement.innerHTML = '<i class="bi bi-check"></i> Copié';
        
        setTimeout(function() {
            btnElement.innerHTML = originalText;
        }, 2000);
    }).catch(function(err) {
        console.error('Erreur lors de la copie: ', err);
    });
}

// Fonctions pour le filtrage dynamique des tableaux
function filterTable(inputId, tableId) {
    var input = document.getElementById(inputId);
    var filter = input.value.toUpperCase();
    var table = document.getElementById(tableId);
    var tr = table.getElementsByTagName('tr');

    for (var i = 0; i < tr.length; i++) {
        var td = tr[i].getElementsByTagName('td');
        var found = false;
        
        for (var j = 0; j < td.length; j++) {
            if (td[j]) {
                var txtValue = td[j].textContent || td[j].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    found = true;
                    break;
                }
            }
        }
        
        if (found) {
            tr[i].style.display = '';
        } else {
            if (i > 0) { // Ne pas cacher l'en-tête
                tr[i].style.display = 'none';
            }
        }
    }
}
