document.addEventListener('DOMContentLoaded', function () {
    const tableBody = document.getElementById('courses-table');

    // Fetch and render courses
    function fetchCourses() {
        fetch('https://localhost:44380/api/Course')
            .then(res => res.json())
            .then(data => {
                tableBody.innerHTML = '';
                data.forEach(course => {
                    // لكل كورس، اجلب عدد الطلاب والسيشنات
                    fetch(`https://localhost:44380/api/Course/${course.id}/counts`)
                        .then(res => res.json())
                        .then(counts => {
                            const tr = document.createElement('tr');
                            tr.innerHTML = `
                                <td>${course.id}</td>
                                <td>${course.title}</td>
                                <td>${course.instructorName}</td>
                                <td>${course.price}</td>
                                <td>${course.duration}</td>
                                <td>
                                    <a href="Students.html?courseId=${course.id}" class="count-link">
                                        ${counts.studentsCount}
                                    </a>
                                </td>
                                <td>
                                    <a href="Sessions.html?courseId=${course.id}" class="count-link">
                                        ${counts.sessionsCount}
                                    </a>
                                </td>

                                <td>
                                    <button class="btn btn-sm btn-info edit-course" data-id="${course.id}">Edit</button>
                                    <button class="btn btn-sm btn-danger delete-course" data-id="${course.id}">Delete</button>
                                </td>
                            `;
                            tableBody.appendChild(tr);
                        });
                });
            })
            .catch(error => {
                tableBody.innerHTML = `<tr><td colspan="7" style="color:red;">${error.message}</td></tr>`;
            });
    }

    fetchCourses();

    // Add course handler (basic, extend as needed)
    document.getElementById('course-form').addEventListener('submit', function (e) {
        e.preventDefault();
        const course = {
            name: document.getElementById('course-name').value,
            category: document.getElementById('course-category').value,
            description: document.getElementById('course-description').value,
            price: parseFloat(document.getElementById('course-price').value),
            duration: parseInt(document.getElementById('course-duration').value, 10),
            imageUrl: document.getElementById('course-image').value
        };

        fetch('https://localhost:44380/api/Course', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(course)
        })
        .then(res => {
            if (!res.ok) throw new Error('Failed to add course');
            return res.json();
        })
        .then(() => {
            fetchCourses();
            e.target.reset();
            alert('Course added successfully!');
        })
        .catch(error => alert(error.message));
    });

    // Delete course handler (basic, extend as needed)
    tableBody.addEventListener('click', function (e) {
        if (e.target.classList.contains('delete-course')) {
            const id = e.target.getAttribute('data-id');
            if (confirm('Are you sure you want to delete this course?')) {
                fetch(`https://localhost:44380/api/Course/${id}`, {
                    method: 'DELETE'
                })
                .then(res => {
                    if (!res.ok) throw new Error('Failed to delete course');
                    fetchCourses();
                })
                .catch(error => alert(error.message));
            }
        }
    });

    // Tab switching logic (optional, for better UX)
    document.querySelectorAll('.tab').forEach(tab => {
        tab.addEventListener('click', function () {
            document.querySelectorAll('.tab').forEach(t => t.classList.remove('active'));
            document.querySelectorAll('.tab-content').forEach(tc => tc.classList.remove('active'));
            tab.classList.add('active');
            document.getElementById(tab.dataset.tab).classList.add('active');
        });
    });
});
// Fetch instructors and populate dropdown
async function loadInstructors() {
    const select = document.getElementById("instructor-id");

    try {
        const response = await fetch("https://localhost:44380/api/Account/instructor");
        if (!response.ok) throw new Error("Failed to fetch instructors");

        const instructors = await response.json();
        select.innerHTML = '<option value="">Select Instructor</option>';

        instructors.forEach(instr => {
            const option = document.createElement("option");
            option.value = instr.id;
            option.textContent = `${instr.name} (${instr.specialization})`;
            select.appendChild(option);
        });
    } catch (error) {
        console.error("Error loading instructors:", error);
        select.innerHTML = '<option value="">Error loading instructors</option>';
    }
}

// Run immediately when page loads
document.addEventListener("DOMContentLoaded", loadInstructors);

document.getElementById("course-form").addEventListener("submit", async (e) => {
    e.preventDefault();

    const course = {
        title: document.getElementById("course-name").value.trim(),
        description: document.getElementById("course-description").value.trim(),
        imageUrl: document.getElementById("course-image").value.trim(),
        duration: document.getElementById("course-duration").value.trim(),
        price: parseFloat(document.getElementById("course-price").value),
        instructorId: parseInt(document.getElementById("instructor-id").value)
    };

    const messageDiv = document.getElementById("response-message");

    try {
        const response = await fetch("https://localhost:44380/api/Course", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(course)
        });

        if (response.ok) {
            const text = await response.text();
            messageDiv.style.color = "green";
            messageDiv.textContent = text || "Course created successfully!";
            e.target.reset();
        } else {
            const error = await response.text();
            messageDiv.style.color = "red";
            messageDiv.textContent = `Error: ${error}`;
        }
    } catch (err) {
        console.error(err);
        messageDiv.style.color = "red";
        messageDiv.textContent = "Network error or server unreachable.";
    }
});
