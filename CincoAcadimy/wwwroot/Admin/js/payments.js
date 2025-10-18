// Pseudocode:
// 1. Fetch payment data from /api/Payments and populate the table.
// 2. Render each payment row with an editable status dropdown and a save button.
// 3. On save, send a PUT request to update the payment status.
// 4. Handle errors and update UI accordingly.

document.addEventListener('DOMContentLoaded', function () {
    const tableBody = document.getElementById('all-payments-table');

    // Fetch payments
    fetch('https://localhost:44380/api/Payments')
        .then(res => res.json())
        .then(data => {
            tableBody.innerHTML = '';
            data.forEach(payment => {
                const tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${payment.id}</td>
                    <td>${payment.phone}</td>
                    <td>${payment.courseId}</td>
                    <td>${payment.coursePrice}</td>
                    <td>${new Date(payment.createdAt).toLocaleString()}</td>
                    <td>
                        <select class="status-select">
                            <option value="Pending" ${payment.status === 'Pending' ? 'selected' : ''}>Pending</option>
                            <option value="Approved" ${payment.status === 'Approved' ? 'selected' : ''}>Approved</option>
                            <option value="Rejected" ${payment.status === 'Rejected' ? 'selected' : ''}>Rejected</option>
                        </select>
                    </td>
                    <td>
                        <button class="btn btn-primary save-status" data-id="${payment.id}">Save</button>
                    </td>
                `;
                tableBody.appendChild(tr);
            });
        });

    // Update status handler
    tableBody.addEventListener('click', function (e) {
        if (e.target.classList.contains('save-status')) {
            const row = e.target.closest('tr');
            const id = e.target.getAttribute('data-id'); // ده هو paymentId
            const status = row.querySelector('.status-select').value;

            fetch(`https://localhost:44380/api/Payments/status`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ paymentId: parseInt(id), status })
            })
                .then(res => {
                    if (res.ok) {
                        e.target.textContent = 'Saved';
                        setTimeout(() => { e.target.textContent = 'Save'; }, 1500);
                    } else {
                        alert('Failed to update status');
                    }
                })
                .catch(err => {
                    console.error('Error updating status:', err);
                    alert('Something went wrong');
                });
        }
    });

<!-- Add this script reference before </body> -->
<script src="js/payments.js"></script>
