let sessionsData = [];
const urlParams = new URLSearchParams(window.location.search);
const courseId = urlParams.get("course");
let sessionId = urlParams.get("session");
const studentId = localStorage.getItem("studentId");
loadSession(Number(sessionId));

fetch(`https://localhost:44380/api/Session/course/${courseId}/student/${studentId}`)
    .then(res => res.json())
    .then(data => {
        sessionsData = data.map((s, index) => ({
            id: s.id,
            title: s.name,
            description: s.description || "No description provided.",
            video: s.videoUrl,
            isCompleted: s.isCompleted, // من الـ API
            number: String(index + 1).padStart(2, '0'),
            resources: (s.resources || []).map(r => ({
                id: r.id,
                label: r.title,
                url: r.url,
                type: r.fileType,
                downloadable: r.isDownloadable
            }))
        }));

        renderList();
        updateCourseSummary(); // <<< هنا
        loadAssessment(sessionId);

        // 👇 هنا يفتح السيشن اللي جاي من الـ URL أوّل ما الصفحة تفتح
        if (sessionId) {
            loadSession(Number(sessionId));

            // 👇 وكمان نعمل highlight للسيشن المفتوح
            document.querySelectorAll(".session-item").forEach(el => {
                el.setAttribute("aria-selected", el.dataset.id == sessionId);
            });
        }
    })
    .catch(err => console.error("Error loading sessions:", err));


let currentSessionId = null;
const sessionsListEl = document.getElementById('sessionsList');
const remainingLabel = document.getElementById('remainingLabel');

function renderList(filter = "") {
    sessionsListEl.innerHTML = '';
    const filtered = sessionsData.filter(s => s.title.toLowerCase().includes(filter.toLowerCase()));

    filtered.forEach((s, index) => {
        const li = document.createElement('div');
        li.className = 'session-item';
        li.dataset.id = s.id;

        const sessionNumber = String(index + 1).padStart(2, '0');

        li.innerHTML = `
        <div class="left-col">
            <div class="thumb" aria-hidden="true">S ${sessionNumber}</div>
            <div class="info">
                <div class="title">${s.title}</div>
            </div>
        </div>
        <div class="right-col">
            <div class="progress-mini" aria-hidden="true">
                <i style="width:${s.isCompleted ? 100 : 0}%"></i>
            </div>
            <div style="display:flex;gap:8px;align-items:center">
                <div class="tag">${s.isCompleted ? "Completed" : "Start Session"}</div>
            </div>
        </div>
        `;

        li.addEventListener('click', () => loadSession(s.id));
        li.addEventListener('keypress', (e) => { if (e.key === 'Enter') loadSession(s.id); });

        sessionsListEl.appendChild(li);
    });

    const remaining = sessionsData.filter(s => !s.isCompleted).length;
    remainingLabel.textContent = remaining + (remaining === 1 ? ' remaining' : ' remaining');
}

function loadSession(id) {
    sessionId = id;
    loadAssessment(sessionId);
    const s = sessionsData.find(x => x.id === id);
    if (!s) return;
    currentSessionId = id;

    document.querySelectorAll('.session-item').forEach(el =>
        el.setAttribute('aria-selected', el.dataset.id == id)
    );

    document.getElementById("contentTitle").textContent = s.title;
    document.getElementById("contentDesc").textContent = s.description;

    const videoWrap = document.getElementById("videoWrap");
    const videoFrame = document.getElementById("videoFrame");
    if (s.video) {
        let videoUrl = s.video.trim();

        if (videoUrl.includes("youtu.be/")) {
            const videoId = videoUrl.split("youtu.be/")[1];
            videoUrl = `https://www.youtube.com/embed/${videoId}`;
        }

        if (videoUrl.includes("watch?v=")) {
            const videoId = videoUrl.split("watch?v=")[1].split("&")[0];
            videoUrl = `https://www.youtube.com/embed/${videoId}`;
        }

        videoFrame.src = videoUrl;
        videoWrap.style.display = "block";
    } else {
        videoWrap.style.display = "block";
    }

    const resourcesDiv = document.getElementById("resources");
    resourcesDiv.innerHTML = "<h3>Resources</h3>";

    s.resources.forEach(r => {
        const res = document.createElement("div");
        res.className = "resource-item";
        res.innerHTML = `
        <div class="resource-info">
            <span>${r.label}</span>
            <small>Type: ${r.type}</small>
        </div>
        ${r.downloadable ? `<a href="${r.url}" download><button class="download-btn">Download</button></a>` : ""}
        `;
        resourcesDiv.appendChild(res);
    });
}

renderList();

document.getElementById("backBtn").addEventListener("click", () => {
    window.location.href = `https://localhost:44380/Student/html/course-sessions.html?id=${courseId}`;
});
function updateCourseSummary() {
    const total = sessionsData.length;
    const completed = sessionsData.filter(s => s.isCompleted).length;

    const summaryEl = document.getElementById("courseSummary");
    const completedEl = document.getElementById("completedCount");

    completedEl.textContent = completed;
    summaryEl.innerHTML = `Course • ${total} sessions • <span id="completedCount">${completed}</span> completed`;
}

async function loadAssessment(sessionId) {
    try {
        const res = await fetch(`https://localhost:44380/api/Assessments/session/${sessionId}`);
        if (!res.ok) throw new Error("Failed to fetch assessment");

        const data = await res.json();
        if (data.length === 0) {
            document.getElementById("assessmentSection").style.display = "none";
            return;
        }

        const assessment = data[0]; // assuming one per session
        const card = document.getElementById("assessmentCard");
        document.getElementById("assessmentSection").style.display = "block";

        card.innerHTML = `
    <div class="assessment-info">
        <strong>${assessment.title}</strong>
        <p>${assessment.description}</p>
        ${assessment.filePath ? `<img src="${assessment.filePath}" alt="Assessment File" style="width: auto;margin-top:10px;border-radius:8px">` : ""}
    </div>
    
    <div class="submit-section" style="margin-top:15px;">
        <input type="text" id="submissionLink-${assessment.id}" placeholder="Paste your solution link here" style="width:80%;padding:6px;border-radius:6px;border:1px solid #ccc;">
        <button class="btn success" onclick="submitSolution(${assessment.id})">Submit Solution</button>
    </div>
`;
    } catch (err) {
        console.error(err);
    }
}

function startAssessment(id) {
    alert("Start assessment with ID: " + id);
    // هنا ممكن توديه لصفحة تانية أو تعمل logic للامتحان
}

loadAssessment(sessionId);

async function submitSolution(assessmentId) {
    const studentId = 5; // هنا هتحط StudentId الحقيقي (ممكن تجيبه من الـ session)
    const submissionLink = document.getElementById(`submissionLink-${assessmentId}`).value;

    if (!submissionLink) {
        alert("Please enter your solution link before submitting!");
        return;
    }

    try {
        const response = await fetch("https://localhost:44380/api/Assessments/upload", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                studentId: studentId,
                assessmentId: assessmentId,
                submissionLink: submissionLink
            })
        });

        if (response.ok) {
            alert("✅ Solution submitted successfully!");
        } else {
            const errorText = await response.text();
            console.error(errorText);
            alert("❌ Error submitting solution: ");
            //alert("❌ Error submitting solution: " + errorText);
        }
    } catch (err) {
        console.error(err);
        alert("⚠️ Network error: " + err.message);
    }
}