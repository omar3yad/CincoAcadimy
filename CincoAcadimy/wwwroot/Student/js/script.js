document.addEventListener("DOMContentLoaded", () => {
    // المكان اللي هيترندر فيه الكورسات
    const ongoingCoursesContainer = document.querySelector(".grid > div > div > div[style]");

    // نجيب البيانات من الـ API
    fetch("https://localhost:44380/api/Course/ongoing/5")
        .then(response => response.json())
        .then(data => {
            // نفضي الكونتينر الأول (لو فيه كورسات ثابتة)
            ongoingCoursesContainer.innerHTML = "";

            data.forEach(course => {
                // نعمل عنصر للكورس
                const courseCard = document.createElement("div");
                courseCard.className = "course-card card";
                courseCard.setAttribute("role", "article");
                courseCard.setAttribute("aria-label", `Course: ${course.title}`);

                // حساب الـ progress و next lesson
                const progress = course.progress || 0;
                const nextLessonName = course.nextLesson ? course.nextLesson.name : "Not Available";

                courseCard.innerHTML = `
                    <div class="course-top">
                        <div class="course-info">
                            <div class="course-thumb">${course.title.slice(0, 2).toUpperCase()}</div>
                            <div>
                                <div style="font-weight:700">${course.title}</div>
                                <div class="muted">by ${course.instructorName}</div>
                            </div>
                        </div>
                        <div style="display:flex;flex-direction:column;align-items:flex-end;gap:8px">
                            <div class="muted">${progress}% complete</div>
                            <div style="width:160px" class="progress" aria-hidden="true"><span style="width:${progress}%"></span></div>
                        </div>
                    </div>
                    <div style="display:flex;justify-content:space-between;align-items:center">
                        <div class="small muted">Next lesson: ${nextLessonName}</div>
                        <button class="continue" onclick="window.location.href='/Student/html/course-sessions.html?id=${course.id}'">Continue</button>
                    </div>
                `;

                ongoingCoursesContainer.appendChild(courseCard);
            });
        })
        .catch(error => {
            console.error("Error fetching courses:", error);
        });
});
