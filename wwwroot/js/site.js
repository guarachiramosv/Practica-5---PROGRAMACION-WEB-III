// Funciones para confirmación de eliminación con SweetAlert2
function confirmarEliminacion(e, titulo, texto) {
    if (e) e.preventDefault();
    const btn = e.currentTarget;
    const form = btn.closest('form');
    
    Swal.fire({
        title: titulo || '¿Estás seguro?',
        text: texto || "¡No podrás revertir esto!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            if (form) {
                form.submit();
            } else {
                console.error("No se encontró el formulario para eliminar.");
            }
        }
    });
}

// Inicializar tooltips de Bootstrap
document.addEventListener('DOMContentLoaded', function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    });
});
