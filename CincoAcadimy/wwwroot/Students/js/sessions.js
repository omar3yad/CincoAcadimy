document.addEventListener("DOMContentLoaded", () => {
    const urlParams = new URLSearchParams(window.location.search);
    const courseId = urlParams.get("id");
    const studentId = localStorage.getItem("studentId");
    const apiUrl = `https://localhost:44380/api/Session/course/${courseId}/student/${studentId}`;

    const courseNameSpan = document.getElementById("courseName");
    const sessionsList = document.getElementById("sessionsList");
        
    async function fetchSessions() {
        try {
            const response = await fetch(apiUrl);
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

            const sessions = await response.json();
            courseNameSpan.textContent = `Course #${courseId}`;

            sessionsList.innerHTML = "";

            if (!sessions || sessions.length === 0) {
                sessionsList.innerHTML = `<p class="muted">No sessions available for this course.</p>`;
                return;
            }

            sessions.forEach(session => {
                const div = document.createElement("div");
                div.className = "session-card";

                const statusText = session.isCompleted ? "Completed ✅" : "Not Completed ❌";
                const btnClass = session.isCompleted ? "completed" : "start";
                const btnText = session.isCompleted ? "Review" : "Start";

                // Resources preview
                let resourcesHtml = "";
                if (session.resources && session.resources.length > 0) {
                    resourcesHtml = `
                        <div class="resources">
                            <strong>Resources:</strong>
                            <ul>
                                ${session.resources.map(r => `
                                    <li>
                                        ${r.title} (${r.fileType})
                                        ${r.isDownloadable
                            ? `<a href="${r.url}" download class="download-link">Download</a>`
                            : `<a href="${r.url}" target="_blank" class="download-link">View</a>`
                        }
                                    </li>
                                `).join("")}
                            </ul>
                        </div>
                    `;
                }

                div.innerHTML = `
                    <div class="session-info">
                        <h2 class="session-title">${session.name}</h2>
                        <p class="session-desc">${session.description || "No description provided."}</p>
                        <p class="session-meta">Status: ${statusText}</p>
                        ${resourcesHtml}
                    </div>
                    <div class="session-actions">
                        <button class="btn ${btnClass}" data-id="${session.id}">
                            ${btnText}
                        </button>
                    </div>
                `;

                // زرار الانتقال لصفحة التفاصيل
                div.querySelector("button").addEventListener("click", () => {
                    window.location.href = `https://localhost:44380/Student/html/session.html?course=${courseId}&session=${session.id}`;
                });

                sessionsList.appendChild(div);
            });

        } catch (error) {
            console.error("Error fetching sessions:", error);
            sessionsList.innerHTML = `<p class="error">⚠️ Error loading sessions. Please try again later.</p>`;
        }
    }

    fetchSessions();
});
