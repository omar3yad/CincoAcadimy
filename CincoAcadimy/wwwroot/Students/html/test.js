document.addEventListener("DOMContentLoaded", async () => {
    const lessonsList = document.getElementById("lessons-list");
    const lessonsCount = document.getElementById("lessons-count");

    // 🧭 Get Course ID from URL
    const urlParams = new URLSearchParams(window.location.search);
    const courseId = urlParams.get("course"); // e.g. 1017

    // 👤 Get Student ID from localStorage
    const studentId = localStorage.getItem("studentId"); // e.g. 8

    if (!courseId || !studentId) {
        lessonsList.innerHTML = `<p style="color:red;">Missing course or student ID.</p>`;
        return;
    }

    try {
        // 🧠 Fetch Lessons from API
        const response = await fetch(`https://localhost:44380/api/Session/course/${courseId}/student/${studentId}`);
        if (!response.ok) throw new Error("Failed to fetch lessons");

        const lessons = await response.json();

        // 🧾 Update lessons count
        lessonsCount.textContent = `${lessons.length} lessons`;

        // 🧱 Clear and populate list
        lessonsList.innerHTML = "";

        lessons.forEach((lesson, index) => {
            const lessonDiv = document.createElement("div");
            lessonDiv.classList.add("lesson-item");
            if (lesson.isCompleted) lessonDiv.classList.add("completed");

            lessonDiv.innerHTML = `
                <div class="lesson-header">
                    <span class="lesson-number">${index + 1}.</span>
                    <h4 class="lesson-title">${lesson.name}</h4>
                </div>
                <p class="lesson-desc">${lesson.description}</p>
                <a href="${lesson.videoUrl}" target="_blank" class="lesson-link">Watch Lesson</a>
            `;

            lessonsList.appendChild(lessonDiv);
        });

    } catch (error) {
        console.error(error);
        lessonsList.innerHTML = `<p style="color:red;">Error loading lessons.</p>`;
    }
});
