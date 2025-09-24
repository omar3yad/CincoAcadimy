document.getElementById('loginForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    const userName = document.getElementById('userName').value;
    const password = document.getElementById('password').value;
    const message = document.getElementById('loginMessage');

    try {
        const res = await fetch('https://localhost:44380/api/Account/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userName, password })
        });

        const data = await res.json();

        if (res.ok) {

            message.style.color = 'green';
            message.innerText = 'Login successful! Role: ' + data.role ;
            // Save token and role in localStorage
            localStorage.setItem("token", data.token); // التوكن اللي جاي من السيرفر
            localStorage.setItem("role", data.role);
            localStorage.setItem("studentId", data.studentId);   // ✅ الحروف صغيرة
            localStorage.setItem("studentName", data.studentName); // ✅ برضو صغيرة


            // Redirect to dashboard based on role
            if (data.role === "Admin") {
                window.location.href = "/frontend/admin/index.html";
            } else if (data.role === "Student") {
                window.location.href = "/Student/html/index.html";
            } else if (data.role === "HR") {
                window.location.href = "/frontend/hr/index.html";
            }
        } else {
            message.style.color = 'red';
            message.innerText = data.error || 'Login failed.';
        }
    } catch (err) {
        message.innerText = 'Error connecting to server.';
    }
});
