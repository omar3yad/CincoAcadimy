document.getElementById('registerForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    const userName = document.getElementById('userName').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const role = document.getElementById('role').value;
    const message = document.getElementById('registerMessage');

    try {
        const res = await fetch('https://localhost:port/api/account/register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userName, email, password, role })
        });

        const data = await res.json();

        if (res.ok) {
            message.style.color = 'green';
            message.innerText = 'Registered successfully.';
        } else {
            message.style.color = 'red';
            message.innerText = data.error || 'Registration failed.';
        }
    } catch (err) {
        message.innerText = 'Error connecting to server.';
    }
});

