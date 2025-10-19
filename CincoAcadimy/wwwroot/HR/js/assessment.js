const sessions = [

];

function showToast(message, isError = false) {
    const toast = document.getElementById('assessmentToast');
    toast.innerHTML = `<i class="fas fa-${isError ? 'times' : 'check'}-circle"></i> ${message}`;
    toast.style.background = isError ? '#e74c3c' : '#3f72af';
    toast.hidden = false;
    setTimeout(() => { toast.hidden = true; }, 2200);
}

// Populate session dropdown
function populateSessions() {
    const select = document.getElementById('sessionSelect');
    select.innerHTML = '<option value="">Select Session</option>';
    sessions.forEach(s => {
        select.innerHTML += `<option value="${s.id}">${s.name}</option>`;
    });
}

// Render assessments table
async function renderAssessments() {
    const tbody = document.getElementById('assessmentTableBody');
    const sessionId = document.getElementById('sessionSelect').value;

    try {
        const res = await fetch("https://localhost:44380/api/Assessments");
        const data = await res.json();

        // فلترة حسب السيشن
        let filtered = sessionId ? data.filter(a => a.sessionId == sessionId) : data;

        if (!filtered.length) {
            tbody.innerHTML = `<tr><td colspan="6">No assessments found.</td></tr>`;
            return;
        }

        tbody.innerHTML = '';
        filtered.forEach(a => {
            tbody.innerHTML += `
                <tr>
                    <td>${a.id}</td>
                    <td>${a.title}</td>
                    <td>${a.description || ''}</td>
                    <td>${a.dueDate || ''}</td>
                    <td>
                        ${a.filePath
                    ? `<a href="${a.filePath}" target="_blank" download>
                                  <i class="fas fa-download"></i> Download
                               </a>`
                    : ''}
                    </td>
                    <td class="actions">
                        <button class="edit-btn" title="Edit" aria-label="Edit" data-id="${a.id}">
                            <i class="fas fa-edit"></i>
                        </button>
                        <button class="delete-btn" title="Delete" aria-label="Delete" data-id="${a.id}">
                            <i class="fas fa-trash"></i>
                        </button>
                    </td>
                </tr>
            `;
        });

    } catch (err) {
        console.error("Error loading assessments:", err);
        tbody.innerHTML = `<tr><td colspan="6">Error loading assessments.</td></tr>`;
    }
}


// Render student submissions table
async function renderSubmissions() {
    const tbody = document.getElementById('submissionTableBody');
    const sessionId = document.getElementById('sessionSelect').value;

    try {
        const res = await fetch("https://localhost:44380/api/Assessments/StudentAssessmen");
        if (!res.ok) {
            tbody.innerHTML = `<tr><td colspan="8">Error loading submissions.</td></tr>`;
            return;
        }
        const submissions = await res.json();

        // Filter submissions by selected session (if needed)
        const filtered = sessionId
            ? submissions.filter(s => s.assessmentId == sessionId)
            : submissions;

        if (!filtered.length) {
            tbody.innerHTML = `<tr><td colspan="8">No submissions found.</td></tr>`;
            return;
        }

        tbody.innerHTML = '';
        filtered.forEach(s => {
            const ungraded = s.grade === -1 || s.grade === null;
            tbody.innerHTML += `
                <tr class="${ungraded ? 'ungraded' : ''}">
                    <td>${s.studentId}</td>
                    <td>${s.studentName || ''}</td>
                    <td>${s.assessmentId}</td>
                    <td>${s.assessmentTitle || ''}</td>
                    <td>${s.submissionLink ? `<a href="${s.submissionLink}" target="_blank"><i class="fas fa-file"></i> View File</a>` : ''}</td>
                    <td>${s.submittedAt ? new Date(s.submittedAt).toLocaleString() : ''}</td>
                    <td>
                        <input type="text" class="grade-input" 
                               value="${s.grade !== -1 ? s.grade : ''}" 
                               data-student="${s.studentId}" 
                               data-assessment="${s.assessmentId}" 
                               aria-label="Grade" style="width:60px;">
                    </td>
                    <td>
                        <button class="save-grade-btn btn" 
                                data-student="${s.studentId}" 
                                data-assessment="${s.assessmentId}" 
                                title="Save Grade" aria-label="Save Grade">
                                <i class="fas fa-save"></i>
                        </button>
                    </td>
                </tr>
            `;
        });
    } catch (err) {
        console.error("Error loading submissions:", err);
        tbody.innerHTML = `<tr><td colspan="8">Failed to load submissions.</td></tr>`;
    }
}

// Form validation and add assessment using API
document.getElementById('assessmentForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    // Get form values
    const title = document.getElementById('title').value.trim();
    const description = document.getElementById('description').value.trim();
    const dueDate = document.getElementById('dueDate').value;
    const sessionId = document.getElementById('sessionSelect').value;
    const fileInput = document.getElementById('file');

    // Validation
    if (!title) {
        showToast('Title is required.', true);
        return;
    }
    if (!dueDate) {
        showToast('Due date is required.', true);
        return;
    }
    if (!sessionId) {
        showToast('Session is required.', true);
        return;
    }

    // Prepare FormData for file upload
    const formData = new FormData();
    formData.append('title', title);
    formData.append('description', description);
    formData.append('dueDate', dueDate);
    formData.append('sessionId', sessionId);

    if (fileInput.files.length > 0) {
        formData.append('file', fileInput.files[0]);
    }

    try {
        const res = await fetch('https://localhost:44380/api/Assessments/add', {
            method: 'POST',
            body: formData
        });

        const text = await res.text();

        let message = 'Assessment added successfully!';
        if (text) {
            try {
                const data = JSON.parse(text);
                message = data.message || message;
            } catch {
                message = text;
            }
        }

        if (!res.ok) {
            showToast(message, true);
            return;
        }

        alert("Assessment added successfully!");
        showToast(message);
        e.target.reset();
        document.getElementById('filePreview').textContent = '';
        renderAssessments();
    } catch (err) {
        showToast('Network error. Please try again.', true);
        console.error(err);
    }
});

// File upload preview
document.getElementById('file').addEventListener('change', function () {
    const file = this.files[0];
    document.getElementById('filePreview').textContent = file ? file.name : '';
});

// Session selector change
document.getElementById('sessionSelect').addEventListener('change', function () {
    renderAssessments();
    renderSubmissions();
});

// Edit assessment
document.getElementById('assessmentTableBody').addEventListener('click', function (e) {
    if (e.target.closest('.edit-btn')) {
        const id = e.target.closest('.edit-btn').dataset.id;
        const a = assessments.find(x => x.id == id);
        if (!a) return;
        document.getElementById('title').value = a.title;
        document.getElementById('description').value = a.description;
        document.getElementById('dueDate').value = a.dueDate;
        document.getElementById('sessionSelect').value = a.sessionId;
        showToast('Loaded for editing. Edit and submit to save.');
        // Remove old assessment on edit submit, will be re-added
        assessments = assessments.filter(x => x.id != id);
        renderAssessments();
        renderSubmissions();
    }
});

// Delete 

// Save grade
document.getElementById('submissionTableBody').addEventListener('click', async function (e) {
    if (e.target.closest('.save-grade-btn')) {
        const btn = e.target.closest('.save-grade-btn');
        const studentId = btn.getAttribute('data-student');
        const assessmentId = btn.getAttribute('data-assessment');
        const input = btn.closest('tr').querySelector('.grade-input');
        const grade = input.value.trim();

        if (!studentId || !assessmentId || grade === '') {
            showToast('Please enter a grade.', true);
            return;
        }

        try {
            const res = await fetch('https://localhost:44380/api/Assessments/update-grade', {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    studentId: parseInt(studentId),
                    assessmentId: parseInt(assessmentId),
                    grade: parseFloat(grade)
                })
            });

            const text = await res.text();
            let message = 'Grade updated successfully.';
            if (text) {
                try {
                    const data = JSON.parse(text);
                    message = data.message || message;
                } catch {
                    message = text;
                }
            }

            if (!res.ok) {
                showToast(message, true);
                return;
            }

            showToast(message);
            renderSubmissions(); // Refresh table
        } catch (err) {
            showToast('Network error. Please try again.', true);
            console.error(err);
        }
    }
});

// Initial load
window.addEventListener('DOMContentLoaded', () => {
    populateSessions();
    renderAssessments();
    renderSubmissions();
});

// Accessibility: close toast with Esc
document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') {
        document.getElementById('assessmentToast').hidden = true;
    }
});


document.addEventListener("DOMContentLoaded", function () {
    const courseSelect = document.createElement("select");
    courseSelect.id = "courseSelect";
    courseSelect.title = "course";
    courseSelect.required = true;
    courseSelect.setAttribute("aria-required", "true");

    // ضيف الكورس سيليكت قبل السيشن سيليكت
    const sessionSelect = document.getElementById("sessionSelect");
    sessionSelect.parentElement.insertBefore(courseSelect, sessionSelect);

    // جلب الكورسات
    fetch("https://localhost:44380/api/Course")
        .then(res => res.json())
        .then(data => {
            courseSelect.innerHTML = `<option value="">Select Course</option>`;
            data.forEach(course => {
                courseSelect.innerHTML += `<option value="${course.id}">${course.title}</option>`;
            });
        })
        .catch(err => console.error("Error loading courses:", err));

    // عند اختيار كورس - هات السيشن
    courseSelect.addEventListener("change", function () {
        const courseId = this.value;
        if (!courseId) {
            sessionSelect.innerHTML = `<option value="">Select Session</option>`;
            return;
        }

        fetch(`https://localhost:44380/api/Session/course/${courseId}`)
            .then(res => res.json())
            .then(data => {
                console.log("Sessions loaded:", data); // ⬅️ شوف هنا في الكونسول

                sessionSelect.innerHTML = `<option value="">Select Session</option>`;
                data.forEach(session => {
                    sessionSelect.innerHTML += `<option value="${session.id}">${session.name}</option>`;
                });
            })
            .catch(err => console.error("Error loading sessions:", err));
    });
});
// Event delegation for delete buttons
document.getElementById("assessmentTableBody").addEventListener("click", async function (e) {
    if (e.target.closest(".delete-btn")) {
        const btn = e.target.closest(".delete-btn");
        const id = btn.getAttribute("data-id");

        if (!confirm("Are you sure you want to delete this assessment?")) return;

        try {
            const res = await fetch(`https://localhost:44380/api/Assessments/${id}`, {
                method: "DELETE"
            });
            const data = await res.json().catch(() => ({}));

            if (res.ok) {
                showToast(data.message || "Assessment deleted successfully!");
                renderAssessments(); // إعادة تحميل الجدول
            } else {
                showToast(data.message || "Error deleting assessment.", true);
            }
        } catch (err) {
            console.error("Error deleting assessment:", err);
            showToast("Network error while deleting assessment.", true);
        }
    }
});
