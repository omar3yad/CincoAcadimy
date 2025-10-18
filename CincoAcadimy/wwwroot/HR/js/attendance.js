// Helper: Show toast notification
function showToast(message) {
    const toast = document.getElementById('attendanceToast');
    toast.innerHTML = `<i class="fas fa-check-circle"></i> ${message}`;
    toast.hidden = false;
    setTimeout(() => { toast.hidden = true; }, 2000);
}

// Fetch courses for dropdown
async function fetchCourses() {
    const select = document.getElementById('courseSelect');
    select.innerHTML = '<option value="">Select Course</option>';
    try {
        const res = await fetch('https://localhost:44380/api/Course');
        const courses = await res.json();
        courses.forEach(course => {
            select.innerHTML += `<option value="${course.id}">${course.title}</option>`;
        });
    } catch (err) {
        select.innerHTML += '<option disabled>Error loading courses</option>';
    }
}

// Fetch sessions for selected course
async function fetchSessions(courseId) {
    const select = document.getElementById('sessionSelect');
    select.innerHTML = '<option value="">Select Session</option>';
    if (!courseId) return;
    try {
        const res = await fetch(`https://localhost:44380/api/Session/course/${courseId}`);
        const sessions = await res.json();
        sessions.forEach(session => {
            select.innerHTML += `<option value="${session.id}">${session.name || session.date}</option>`;
        });
    } catch (err) {
        select.innerHTML += '<option disabled>Error loading sessions</option>';
    }
}

// Fetch students for selected course/session
async function fetchStudents(courseId, sessionId, search = '') {
    const tbody = document.getElementById('attendanceTableBody');
    tbody.innerHTML = '<tr><td colspan="4">loading</td></tr>';
    try {
        // Get students enrolled in the course using the new endpoint
        const res = await fetch(`https://localhost:44380/api/Course/${courseId}/students`);
        let students = await res.json();

        // If sessionId is provided, filter students by session if needed (optional, depends on backend)
        // If your backend returns only students for the course, you may skip sessionId filtering here.

        // Filter by search
        if (search) {
            students = students.filter(s =>
                s.fullName.toLowerCase().includes(search) ||
                s.id.toLowerCase().includes(search)
            );
        }
        renderAttendanceTable(students, sessionId);
    } catch (err) {
        tbody.innerHTML = '<tr><td colspan="4">Error loading students</td></tr>';
    }
}

// Render attendance table rows with modern select for Status
async function renderAttendanceTable(data, sessionId) {
    const tbody = document.getElementById('attendanceTableBody');
    tbody.innerHTML = '';

    if (!data || data.length === 0) {
        tbody.innerHTML = '<tr><td colspan="4">No students found</td></tr>';
        return;
    }

    for (const student of data) {
        // Call backend to check attendance for this student/session
        let isPresent = false;
        try {
            const res = await fetch(`https://localhost:44380/api/Session/${sessionId}/student/${student.id}/attendance`);
            if (res.ok) {
                const att = await res.json();
                isPresent = att.isCompleted;
            }
        } catch (err) {
            console.warn(`Attendance fetch failed for student ${student.id}`, err);
        }

        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td data-label="Student ID">${student.id}</td>
            <td data-label="Student Name">${student.fullName}</td>
            <td data-label="Status">
                <select class="status-select">
                    <option value="present" ${isPresent ? 'selected' : ''}>✅ Present</option>
                    <option value="absent" ${!isPresent ? 'selected' : ''}>❌ Absent</option>
                </select>
            </td>
            <td data-label="Notes">
                <input type="text" class="notes-input" aria-label="Notes" value="${student.notes || ''}" placeholder="Add note...">
            </td>
        `;
        tbody.appendChild(tr);
    }
}

// Mark All Present
document.getElementById('markAllPresent').addEventListener('click', () => {
    document.querySelectorAll('.status-select').forEach(select => {
        select.value = "present";
    });
});

// Mark All Absent
document.getElementById('markAllAbsent').addEventListener('click', () => {
    document.querySelectorAll('.status-select').forEach(select => {
        select.value = "absent";
    });
});

// Save Attendance
// Save Attendance
document.getElementById('saveAttendance').addEventListener('click', async () => {
    const sessionId = document.getElementById('sessionSelect').value;
    const rows = document.querySelectorAll('#attendanceTableBody tr');
    let success = true;

    for (const row of rows) {
        const studentId = row.children[0].textContent;

        // ناخد القيمة من select بدل الـ checkbox
        const statusSelect = row.querySelector('.status-select');
        const isCompleted = statusSelect.value === "present"; // ✅ present → true, absent → false

        try {
            const res = await fetch(`https://localhost:44380/api/Session/${sessionId}/student/${studentId}/completion`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ isCompleted })
            });

            if (!res.ok) success = false;
        } catch (err) {
            success = false;
        }
    }

    showToast(success ? 'Attendance saved successfully!' : 'Error saving attendance.');
});
// Filter functionality
document.getElementById('filterForm').addEventListener('submit', function (e) {
    e.preventDefault();
    const courseId = document.getElementById('courseSelect').value;
    const sessionId = document.getElementById('sessionSelect').value;
    const search = document.getElementById('studentSearch').value.toLowerCase();
    if (!courseId || !sessionId) return;
    fetchStudents(courseId, sessionId, search);
});

// On course change, fetch sessions
document.getElementById('courseSelect').addEventListener('change', function () {
    const courseId = this.value;
    fetchSessions(courseId);
    document.getElementById('attendanceTableBody').innerHTML = '';
    document.getElementById('sessionSelect').value = '';
});

// On session change, fetch students
document.getElementById('sessionSelect').addEventListener('change', function () {
    const courseId = document.getElementById('courseSelect').value;
    const sessionId = this.value;
    if (courseId && sessionId) fetchStudents(courseId, sessionId);
});

// Initial load: fetch courses
window.addEventListener('DOMContentLoaded', () => {
    fetchCourses();
});

// Accessibility: close toast with Esc
document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') {
        document.getElementById('attendanceToast').hidden = true;
    }
});

// Helper: Show toast notification
function showToast(message, isError = false) {
    const toast = document.getElementById('attendanceToast');
    const msg = document.getElementById('toastMessage');

    msg.textContent = message;
    toast.style.background = isError ? "#e74c3c" : "#2ecc71"; // أحمر لو Error، أخضر لو Success

    toast.hidden = false;
    toast.classList.add("show");

    setTimeout(() => {
        toast.classList.remove("show");
        setTimeout(() => (toast.hidden = true), 300); // يختفي بعد الأنيميشن
    }, 2000);
}
