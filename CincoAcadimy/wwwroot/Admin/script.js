const apiUrl = 'https://localhost:44380/api/Course';
const courseTableBody = document.getElementById('courseTableBody');
const addCourseForm = document.getElementById('addCourseForm');

// Fetch all courses
async function getCourses() {
    try {
        const response = await fetch(apiUrl, {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token') // if using JWT
            }
        });
        const courses = await response.json();
        renderCourses(courses);
    } catch (error) {
        console.error('Error fetching courses:', error);
    }
}

// Render courses in table
function renderCourses(courses) {
    courseTableBody.innerHTML = '';
    courses.forEach(course => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${course.id}</td>
            <td>${course.title}</td>
            <td>${course.description}</td>
            <td>${course.instructorName}</td>
            <td class="actions">
                <button class="edit" onclick="editCourse(${course.id})">Edit</button>
                <button onclick="deleteCourse(${course.id})">Delete</button>
            </td>
        `;
        courseTableBody.appendChild(tr);
    });
}

// Add new course
addCourseForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    const newCourse = {
        title: document.getElementById('title').value,
        description: document.getElementById('description').value,
        instructorName: document.getElementById('instructorName').value
    };
    try {
        const response = await fetch(apiUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem('token')
            },
            body: JSON.stringify(newCourse)
        });
        if (response.ok) {
            alert('Course added!');
            addCourseForm.reset();
            getCourses();
        }
    } catch (error) {
        console.error('Error adding course:', error);
    }
});

// Delete course
async function deleteCourse(id) {
    if (!confirm('Are you sure you want to delete this course?')) return;
    try {
        const response = await fetch(`${apiUrl}/${id}`, {
            method: 'DELETE',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token')
            }
        });
        if (response.ok) {
            alert('Course deleted!');
            getCourses();
        }
    } catch (error) {
        console.error('Error deleting course:', error);
    }
}

// Edit course (simple prompt method)
async function editCourse(id) {
    const title = prompt('Enter new title:');
    const description = prompt('Enter new description:');
    const instructorName = prompt('Enter new instructor name:');
    if (!title || !description || !instructorName) return;

    const updatedCourse = { id, title, description, instructorName };
    try {
        const response = await fetch(`${apiUrl}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem('token')
            },
            body: JSON.stringify(updatedCourse)
        });
        if (response.ok) {
            alert('Course updated!');
            getCourses();
        }
    } catch (error) {
        console.error('Error updating course:', error);
    }
}

// Initial fetch
getCourses();
