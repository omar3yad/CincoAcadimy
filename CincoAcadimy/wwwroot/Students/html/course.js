// Course Details Page JavaScript
document.addEventListener('DOMContentLoaded', function () {
    // Initialize the course page
    initCoursePage();

    // Set up event listeners
    setupCourseEventListeners();

    // Load course data
    loadCourseData();
});

// Course data structure
let currentCourse = null;
const studentId = localStorage.getItem("studentId");

let currentLesson = null;
let lessons = [];

// API Configuration
const API_BASE_URL = 'https://localhost:44380/api';

// Initialize course page
function initCoursePage() {
    console.log('Course page initialized');

    // Get course ID from URL parameters
    const urlParams = new URLSearchParams(window.location.search);
    const courseId = urlParams.get('course') || urlParams.get('id') || '1017';

    // Set course ID for API calls
    window.courseId = courseId;

    // Update navigation active state
    setActiveMenuItem();
}

// Set up event listeners for course page
function setupCourseEventListeners() {
    // Lesson item clicks
    document.addEventListener('click', function (e) {
        if (e.target.closest('.lesson-item')) {
            const lessonItem = e.target.closest('.lesson-item');
            const lessonId = lessonItem.dataset.lessonId;
            selectLesson(lessonId);
        }
    });

    // Navigation buttons
    const prevLessonBtn = document.getElementById('prev-lesson');
    const nextLessonBtn = document.getElementById('next-lesson');
    const markCompleteBtn = document.getElementById('mark-complete');

    if (prevLessonBtn) {
        prevLessonBtn.addEventListener('click', navigateToPreviousLesson);
    }

    if (nextLessonBtn) {
        nextLessonBtn.addEventListener('click', navigateToNextLesson);
    }

    if (markCompleteBtn) {
        markCompleteBtn.addEventListener('click', toggleLessonCompletion);
    }

    // Back to dashboard
    const breadcrumbLink = document.querySelector('.breadcrumb-link');
    if (breadcrumbLink) {
        breadcrumbLink.addEventListener('click', function (e) {
            //e.preventDefault();
            window.location.href = 'student.html';
        });
    }
}

// Load course data from API
async function loadCourseData() {
    try {
        // Show loading state
        showLoadingState();

        // Fetch course data and lessons in parallel
        const [courseData, lessonsData] = await Promise.all([
            fetchCourseData(window.courseId),
            fetchLessonsFromAPI()
        ]);

        // Hide loading state
        hideLoadingState();

        // Process and display data
        displayCourseData(courseData);
        displayLessons(lessonsData);

    } catch (error) {
        console.error('Error loading course data:', error);
        showErrorState();
    }
}

// Fetch course data (mock implementation - you might need a real API for this)
// ✅ Fetch course data from real backend API
async function fetchCourseData(courseId) {
    try {
        const response = await fetch(`https://localhost:44380/api/Course/${courseId}`);

        if (!response.ok) throw new Error(`Failed to fetch course with ID ${courseId}`);

        const course = await response.json();

        // You can extend it if you also have session/lesson data elsewhere
        // (e.g., from StudentSessions or CourseSessions API)

        // Normalize fields to match your frontend structure
        return {
            id: course.id,
            title: course.title,
            instructorName: course.instructorName || "Unknown",
            description: course.description || "No description available",
            thumbnail: course.imageUrl,
            progress: course.progress || 0,
            totalLessons: course.totalLessons || 0, // if available
            completedLessons: course.completedLessons || 0, // if available
            totalDuration: `${course.duration} min`, // duration from API
            price: course.price || 0
        };
    } catch (error) {
        console.error("Error fetching course data:", error);
        return null;
    }
}


// Fetch lessons from real API
async function fetchLessonsFromAPI() {
    const lessonsList = document.getElementById("lessons-list");
    const lessonsCount = document.getElementById("lessons-count");

    // Get Course ID from URL
    const urlParams = new URLSearchParams(window.location.search);
    const courseId = urlParams.get("course") || window.courseId;

    // Get Student ID from localStorage
    const studentId = localStorage.getItem("studentId");

    if (!courseId || !studentId) {
        throw new Error('Missing course or student ID');
    }

    try {
        // Fetch Lessons from API
        const response = await fetch(`${API_BASE_URL}/Session/course/${courseId}/student/${studentId}`);
        if (!response.ok) throw new Error("Failed to fetch lessons");

        const apiLessons = await response.json();

        console.log('Fetched lessons from API:', apiLessons);

        // Transform API data to match our lesson structure
        return apiLessons.map((lesson, index) => ({
            id: lesson.id || index + 1,
            title: lesson.name || `Lesson ${index + 1}`,
            description: lesson.description || 'No description available',
            duration: lesson.duration || '30m',
            completed: lesson.isCompleted || false,
            videoUrl: lesson.videoUrl || '#',
            resources: lesson.resources || []
        }));

    } catch (error) {
        console.error('Error fetching lessons from API:', error);
        // Fallback to mock data if API fails
        return getFallbackLessons(courseId);
    }
}

// Fallback mock data
function getFallbackLessons(courseId) {
    const fallbackLessons = {
        '1017': [
            {
                id: 1, title: 'Introduction to React', duration: '25m', completed: true,
                description: 'Learn the fundamentals of React including components, props, and state.',
                videoUrl: '#', resources: []
            },
            {
                id: 2, title: 'Components and Props', duration: '35m', completed: true,
                description: 'Deep dive into React components and how to pass data using props.',
                videoUrl: '#', resources: []
            }
        ]
    };
    return fallbackLessons[courseId] || [];
}

// Display course data in the UI
function displayCourseData(courseData) {
    currentCourse = courseData;

    // Update course header
    const courseTitle = document.getElementById('course-title');
    const courseDescription = document.getElementById('course-description');
    const courseInstructor = document.getElementById('course-instructor');
    const courseProgress = document.getElementById('course-progress');
    const courseProgressBar = document.getElementById('course-progress-bar');
    const courseThumbnail = document.getElementById('course-thumbnail');

    if (courseTitle) courseTitle.textContent = courseData.title;
    if (courseDescription) courseDescription.textContent = courseData.description;
    if (courseInstructor) courseInstructor.innerHTML = `<i class="fas fa-user"></i>Instructor: ${courseData.instructorName}`;
    if (courseProgress) courseProgress.innerHTML = `<i class="fas fa-chart-line"></i>${courseData.progress}% Complete`;
    if (courseProgressBar) courseProgressBar.style.width = `${courseData.progress}%`;
    if (courseThumbnail) courseThumbnail.src = courseData.thumbnail;

    // Update welcome message stats
    const totalLessonsElem = document.getElementById('total-lessons');
    const completedLessonsElem = document.getElementById('completed-lessons');
    const totalDurationElem = document.getElementById('total-duration');

    if (totalLessonsElem) totalLessonsElem.textContent = courseData.totalLessons;
    if (completedLessonsElem) completedLessonsElem.textContent = courseData.completedLessons;
    if (totalDurationElem) totalDurationElem.textContent = courseData.totalDuration;
}

// Display lessons in the sidebar
function displayLessons(lessonsData) {
    lessons = lessonsData;
    const lessonsList = document.getElementById("lessons-list");
    const lessonsCount = document.getElementById("lessons-count");

    if (!lessonsList) return;

    // Calculate total duration and completed lessons
    const totalDuration = calculateTotalDuration(lessons);
    const completedLessons = lessons.filter(lesson => lesson.completed).length;

    // Update lessons count
    if (lessonsCount) {
        lessonsCount.textContent = `${lessons.length} lessons • ${totalDuration}`;
    }

    // Update completed lessons in course data
    if (currentCourse) {
        currentCourse.completedLessons = completedLessons;
        currentCourse.totalLessons = lessons.length;
        currentCourse.progress = Math.round((completedLessons / lessons.length) * 100);

        // Update progress display
        const courseProgress = document.getElementById('course-progress');
        const courseProgressBar = document.getElementById('course-progress-bar');
        const completedLessonsElem = document.getElementById('completed-lessons');
        const totalLessonsElem = document.getElementById('total-lessons');

        if (courseProgress) courseProgress.innerHTML = `<i class="fas fa-chart-line"></i>${currentCourse.progress}% Complete`;
        if (courseProgressBar) courseProgressBar.style.width = `${currentCourse.progress}%`;
        if (completedLessonsElem) completedLessonsElem.textContent = completedLessons;
        if (totalLessonsElem) totalLessonsElem.textContent = lessons.length;
    }

    // Clear existing lessons
    lessonsList.innerHTML = "";

    if (lessons.length === 0) {
        lessonsList.innerHTML = '<p class="no-lessons">No lessons available for this course.</p>';
        return;
    }

    // Add lessons to the list
    lessons.forEach((lesson, index) => {
        const lessonDiv = document.createElement("div");
        lessonDiv.classList.add("lesson-item");
        lessonDiv.dataset.lessonId = lesson.id;

        if (lesson.completed) lessonDiv.classList.add("completed");

        lessonDiv.innerHTML = `
            <div class="lesson-header">
                <div>
                    <div class="lesson-title">${lesson.title}</div>
                    <div class="lesson-duration">${lesson.duration}</div>
                </div>
                <span class="lesson-status ${lesson.completed ? 'completed' : ''}">
                    ${lesson.completed ? 'Completed' : 'Not Started'}
                </span>
            </div>

        `;

        lessonsList.appendChild(lessonDiv);
    });

    // Select the first incomplete lesson, or the first lesson if all are complete
    const firstIncompleteLesson = lessons.find(lesson => !lesson.completed);
    const firstLesson = lessons[0];
    const lessonToSelect = firstIncompleteLesson || firstLesson;

    if (lessonToSelect) {
        selectLesson(lessonToSelect.id);
    }
}

// Calculate total duration from lessons
function calculateTotalDuration(lessons) {
    const totalMinutes = lessons.reduce((total, lesson) => {
        const duration = lesson.duration || '0m';
        const minutes = parseInt(duration) || 0;
        return total + minutes;
    }, 0);

    const hours = Math.floor(totalMinutes / 60);
    const minutes = totalMinutes % 60;

    if (hours > 0) {
        return `${hours}h ${minutes}m`;
    } else {
        return `${minutes}m`;
    }
}

// Select a lesson to display
function selectLesson(lessonId) {
    const lesson = lessons.find(l => l.id == lessonId);
    if (!lesson) return;

    currentLesson = lesson;

    // Update lesson list active state
    document.querySelectorAll('.lesson-item').forEach(item => {
        item.classList.remove('active');
        if (item.dataset.lessonId == lessonId) {
            item.classList.add('active');
        }
    });

    // Show lesson content
    showLessonContent(lesson);

    // Update navigation buttons
    updateNavigationButtons();
    console.log("Fetching assessments for session:", lesson.id);
    fetchAssessments(lesson.id);

    
}

// Show lesson content
function showLessonContent(lesson) {
    const welcomeMessage = document.getElementById('welcome-message');
    const lessonDetails = document.getElementById('lesson-details');
    const videoPlaceholder = document.getElementById('video-placeholder');
    const videoPlayer = document.getElementById('video-player');

    // Hide welcome message, show lesson details
    if (welcomeMessage) welcomeMessage.style.display = 'none';
    if (lessonDetails) lessonDetails.style.display = 'block';

    // Show video player or placeholder based on videoUrl
    if (lesson.videoUrl && lesson.videoUrl !== '#') {
        if (videoPlaceholder) videoPlaceholder.style.display = 'none';
        if (videoPlayer) {
            videoPlayer.style.display = 'block';
            loadVideoPlayer(lesson.videoUrl);
        }
    } else {
        if (videoPlaceholder) {
            videoPlaceholder.style.display = 'flex';
            videoPlaceholder.innerHTML = `
                <i class="fas fa-play-circle"></i>
                <p>No video available for this lesson</p>
            `;
        }
        if (videoPlayer) videoPlayer.style.display = 'none';
    }

    // Update lesson details
    const lessonTitle = document.getElementById('lesson-title');
    const lessonDescription = document.getElementById('lesson-description');

    if (lessonTitle) lessonTitle.textContent = lesson.title;
    if (lessonDescription) lessonDescription.textContent = lesson.description;

    // Update resources
    displayResources(lesson.resources);

    // Update mark complete button
    const markCompleteBtn = document.getElementById('mark-complete');
    if (markCompleteBtn) {
        markCompleteBtn.innerHTML = lesson.completed ?
            '<i class="fas fa-redo"></i> Mark as Incomplete' :
            '<i class="fas fa-check"></i> Mark as Complete';
    }
}

// Load video player
// Load video player
function loadVideoPlayer(videoUrl) {
    const videoContainer = document.querySelector('.video-container');
    if (!videoContainer) return;

    // تنظيف الحاوية أولاً
    videoContainer.innerHTML = '';

    // التحقق إذا كان رابط الفيديو صالحاً
    if (videoUrl && videoUrl !== '#' && isValidVideoUrl(videoUrl)) {
        // إذا كان رابط فيديو مباشر (MP4, WebM, etc)
        if (videoUrl.match(/\.(mp4|webm|ogg|mov|avi|mkv)$/i)) {
            videoContainer.innerHTML = `
                <video controls width="100%" height="100%">
                    <source src="${videoUrl}" type="video/mp4">
                    Your browser does not support the video tag.
                </video>
            `;
        }
        // إذا كان رابط يوتيوب
        else if (videoUrl.includes('youtube.com') || videoUrl.includes('youtu.be')) {
            const videoId = getYouTubeId(videoUrl);
            videoContainer.innerHTML = `
                <iframe 
                    width="100%" 
                    height="400" 
                    src="https://www.youtube.com/embed/${videoId}" 
                    frameborder="0" 
                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" 
                    allowfullscreen>
                </iframe>
            `;
        }
        // إذا كان رابط Vimeo
        else if (videoUrl.includes('vimeo.com')) {
            const videoId = videoUrl.split('/').pop();
            videoContainer.innerHTML = `
                <iframe 
                    src="https://player.vimeo.com/video/${videoId}" 
                    width="100%" 
                    height="400" 
                    frameborder="0" 
                    allow="autoplay; fullscreen; picture-in-picture" 
                    allowfullscreen>
                </iframe>
            `;
        }
        // إذا كان نوع غير معروف، نعرض زر للفتح في نافذة جديدة
        else {
            videoContainer.innerHTML = `
                <div class="video-placeholder-large">
                    <i class="fas fa-play-circle"></i>
                    <p>Video Content</p>
                    <p class="video-info">This video will open in a new window</p>
                    <a href="${videoUrl}" target="_blank" class="btn-primary" style="margin-top: 1rem;">
                        <i class="fas fa-external-link-alt"></i> Watch Video
                    </a>
                </div>
            `;
        }
    } else {
        // إذا لم يكن هناك رابط فيديو صالح
        videoContainer.innerHTML = `
            <div class="video-placeholder-large">
                <i class="fas fa-video-slash"></i>
                <p>No Video Available</p>
                <p class="video-info">Video content is not available for this lesson</p>
            </div>
        `;
    }
}

// دالة لاستخراج ID من رابط يوتيوب
function getYouTubeId(url) {
    const regExp = /^.*((youtu.be\/)|(v\/)|(\/u\/\w\/)|(embed\/)|(watch\?))\??v?=?([^#&?]*).*/;
    const match = url.match(regExp);
    return (match && match[7].length === 11) ? match[7] : false;
}

// دالة للتحقق من صحة رابط الفيديو
function isValidVideoUrl(url) {
    if (!url || url === '#' || url === '') return false;

    // قائمة بالأنواع المسموحة
    const videoPatterns = [
        /\.(mp4|webm|ogg|mov|avi|mkv)$/i,
        /youtube\.com|youtu\.be/i,
        /vimeo\.com/i,
        /\.(m3u8)$/i // للبث المباشر
    ];

    return videoPatterns.some(pattern => pattern.test(url));
}
// Display lesson resources
function displayResources(resources) {
    const resourcesList = document.getElementById('resources-list');
    if (!resourcesList) return;

    resourcesList.innerHTML = '';

    if (!resources || resources.length === 0) {
        resourcesList.innerHTML = '<p class="no-resources">No resources available for this lesson.</p>';
        return;
    }

    resources.forEach(resource => {
        const resourceItem = document.createElement('div');
        resourceItem.className = 'resource-item';

        const icon = getResourceIcon(resource.type);

        resourceItem.innerHTML = `
            <div class="resource-icon">
                <i class="${icon}"></i>
            </div>
            <div class="resource-info">
                <div class="resource-title">${resource.title || 'Untitled Resource'}</div>
                <div class="resource-type">${resource.type ? resource.type.toUpperCase() : 'FILE'}</div>
            </div>
            <a href="${resource.url || '#'}" class="resource-download" ${resource.url ? 'download' : ''}>
                <i class="fas fa-download"></i>
            </a>
        `;

        resourcesList.appendChild(resourceItem);
    });
}

// Get icon for resource type
function getResourceIcon(type) {
    const icons = {
        'pdf': 'fas fa-file-pdf',
        'code': 'fas fa-code',
        'video': 'fas fa-video',
        'document': 'fas fa-file-alt',
        'image': 'fas fa-image'
    };

    return icons[type] || 'fas fa-file';
}

// Update navigation buttons state
function updateNavigationButtons() {
    if (!currentLesson) return;

    const currentIndex = lessons.findIndex(l => l.id === currentLesson.id);
    const prevLessonBtn = document.getElementById('prev-lesson');
    const nextLessonBtn = document.getElementById('next-lesson');

    if (prevLessonBtn) {
        prevLessonBtn.disabled = currentIndex === 0;
    }

    if (nextLessonBtn) {
        nextLessonBtn.disabled = currentIndex === lessons.length - 1;
    }
}

// Navigate to previous lesson
function navigateToPreviousLesson() {
    if (!currentLesson) return;

    const currentIndex = lessons.findIndex(l => l.id === currentLesson.id);
    if (currentIndex > 0) {
        selectLesson(lessons[currentIndex - 1].id);
    }
}

// Navigate to next lesson
function navigateToNextLesson() {
    if (!currentLesson) return;

    const currentIndex = lessons.findIndex(l => l.id === currentLesson.id);
    if (currentIndex < lessons.length - 1) {
        selectLesson(lessons[currentIndex + 1].id);
    }
}

// Toggle lesson completion status
async function toggleLessonCompletion() {
    if (!currentLesson) return;

    try {
        // Get student ID from localStorage
        const studentId = localStorage.getItem("studentId");

        if (!studentId) {
            console.warn('No student ID found in localStorage. Cannot update completion status.');
            return;
        }

        // Toggle completion status locally first for immediate UI feedback
        currentLesson.completed = !currentLesson.completed;

        // Update the lesson in the lessons array
        const lessonIndex = lessons.findIndex(l => l.id === currentLesson.id);
        if (lessonIndex !== -1) {
            lessons[lessonIndex].completed = currentLesson.completed;
        }

        // Update UI
        updateLessonCompletionUI();

        // Send update to API (you would need to implement this endpoint)
        await updateSessionCompletion(studentId, currentLesson.id, currentLesson.completed);

    } catch (error) {
        console.error('Error updating lesson completion:', error);
        // Revert the change if API call fails
        currentLesson.completed = !currentLesson.completed;
        updateLessonCompletionUI();
    }
}

// Update lesson completion UI
function updateLessonCompletionUI() {
    if (!currentLesson) return;

    const lessonItem = document.querySelector(`.lesson-item[data-lesson-id="${currentLesson.id}"]`);
    if (lessonItem) {
        lessonItem.classList.toggle('completed', currentLesson.completed);

        const statusElement = lessonItem.querySelector('.lesson-status');
        if (statusElement) {
            statusElement.textContent = currentLesson.completed ? 'Completed' : 'Not Started';
            statusElement.classList.toggle('completed', currentLesson.completed);
        }
    }

    // Update mark complete button
    const markCompleteBtn = document.getElementById('mark-complete');
    if (markCompleteBtn) {
        markCompleteBtn.innerHTML = currentLesson.completed ?
            '<i class="fas fa-redo"></i> Mark as Incomplete' :
            '<i class="fas fa-check"></i> Mark as Complete';
    }

    // Update course progress
    updateCourseProgress();
}

// Update session completion via API (placeholder)
async function updateSessionCompletion(studentId, sessionId, isCompleted) {
    // This is a placeholder for the API call to update session completion
    // You would need to implement this endpoint on your backend
    const apiUrl = `${API_BASE_URL}/Session/${sessionId}/student/${studentId}/complete`;

    try {
        const response = await fetch(apiUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ isCompleted })
        });

        if (!response.ok) {
            throw new Error(`Failed to update session completion: ${response.status}`);
        }

        console.log(`Session ${sessionId} completion updated to: ${isCompleted}`);
    } catch (error) {
        console.error('Error updating session completion:', error);
        throw error;
    }
}

// Update course progress
function updateCourseProgress() {
    if (!currentCourse) return;

    const completedLessons = lessons.filter(lesson => lesson.completed).length;
    const totalLessons = lessons.length;
    const newProgress = Math.round((completedLessons / totalLessons) * 100);

    // Update course progress
    currentCourse.progress = newProgress;
    currentCourse.completedLessons = completedLessons;

    // Update UI
    const courseProgress = document.getElementById('course-progress');
    const courseProgressBar = document.getElementById('course-progress-bar');
    const completedLessonsElem = document.getElementById('completed-lessons');

    if (courseProgress) courseProgress.innerHTML = `<i class="fas fa-chart-line"></i>${newProgress}% Complete`;
    if (courseProgressBar) courseProgressBar.style.width = `${newProgress}%`;
    if (completedLessonsElem) completedLessonsElem.textContent = completedLessons;

    console.log(`Course progress updated to ${newProgress}%`);
}

// Set active menu item
function setActiveMenuItem() {
    const menuItems = document.querySelectorAll('.menu-item');
    menuItems.forEach(item => {
        item.classList.remove('active');
        if (item.querySelector('span').textContent === 'My Courses') {
            item.classList.add('active');
        }
    });
}

// UI State Management
function showLoadingState() {
    const loadingState = document.createElement('div');
    loadingState.id = 'course-loading';
    loadingState.className = 'loading-state';
    loadingState.innerHTML = `
        <div class="loading-spinner"></div>
        <p>Loading course content...</p>
    `;

    const mainContent = document.querySelector('.main-content');
    if (mainContent) {
        mainContent.appendChild(loadingState);
    }
}

function hideLoadingState() {
    const loadingState = document.getElementById('course-loading');
    if (loadingState) {
        loadingState.remove();
    }
}

function showErrorState() {
    const errorState = document.createElement('div');
    errorState.id = 'course-error';
    errorState.className = 'error-state';
    errorState.innerHTML = `
        <i class="fas fa-exclamation-triangle"></i>
        <h3>Unable to Load Course</h3>
        <p>There was a problem loading the course content. Please try again later.</p>
        <button id="retry-course-btn" class="btn-primary">Retry</button>
    `;  

    const mainContent = document.querySelector('.main-content');
    if (mainContent) {
        mainContent.appendChild(errorState);

        // Add retry event listener
        document.getElementById('retry-course-btn').addEventListener('click', function () {
            errorState.remove();
            loadCourseData();
        });
    }
}
// =============================
// 🔸 Fetch and Display Assessments
// =============================
async function fetchAssessments(sessionId) {
    try {
        const res = await fetch(`https://localhost:44380/api/Assessments/session/${sessionId}`);
        if (!res.ok) throw new Error("Failed to fetch assessment");

        const data = await res.json();
        const section = document.getElementById("assessmentSection");
        const card = document.getElementById("assessmentCard");

        if (data.length === 0) {
            section.style.display = "none";
            return;
        }

        section.style.display = "block";

        card.innerHTML = data.map(assessment => {
            let filePreview = "";

            if (assessment.filePath) {
                const lowerPath = assessment.filePath.toLowerCase();
                if (lowerPath.endsWith(".jpg") || lowerPath.endsWith(".jpeg") || lowerPath.endsWith(".png") || lowerPath.endsWith(".gif")) {
                    // صورة
                    filePreview = `<img src="${assessment.filePath}" alt="Assessment File" 
                        style="max-width:250px;margin-top:10px;border-radius:8px">`;
                } else {
                    // أي حاجة تانية = PDF / DOCX / PPTX / ZIP ...
                    filePreview = `<a href="${assessment.filePath}" target="_blank" 
                        class="btn preview" style="display:inline-block;margin-top:10px;">Preview File</a>`;
                }
            }

            return `
                <div class="assessment-card" style="border:1px solid #ddd;padding:10px;margin-bottom:12px;border-radius:8px;">
                    <div class="assessment-info">
                        <strong>${assessment.title}</strong>
                        <p>${assessment.description || ""}</p>
                        ${filePreview}
                    </div>
                    
                    <div class="submit-section" style="margin-top:15px;">
                        <input type="text" id="submissionLink-${assessment.id}" 
                               placeholder="Paste your solution link here" 
                               style="width:80%;padding:6px;border-radius:6px;border:1px solid #ccc;">
                        <button class="btn success" onclick="submitSolution(${assessment.id})">Submit Solution</button>
                    </div>
                </div>
            `;
        }).join("");

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
    //const studentId = 5; // هنا هتحط StudentId الحقيقي (ممكن تجيبه من الـ session)
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
            alert("⚠️ You have already submitted this assessment.");
            //alert("❌ Error submitting solution: " + errorText);
        }
    } catch (err) {
        console.error(err);
        alert("⚠️ Network error: " + err.message);
    }
}
function displayAssessments(assessments) {
    const list = document.getElementById('assessments-list');
    list.innerHTML = '';

    if (!assessments || assessments.length === 0) {
        list.innerHTML = '<p class="no-assessments">No assessments available for this session.</p>';
        return;
    }

    assessments.forEach(assess => {
        const item = document.createElement('div');
        item.classList.add('assessment-item');
        item.innerHTML = `
            <div class="assessment-header">
                <h4>${assess.title}</h4>
                <span class="due-date"><i class="fas fa-calendar-alt"></i> Due: ${new Date(assess.dueDate).toLocaleDateString()}</span>
            </div>
            <p class="assessment-desc">${assess.description || 'No description provided.'}</p>
            ${assess.filePath
                ? `<a href="${assess.filePath}" class="btn-download" download>
                      <i class="fas fa-download"></i> Download File
                   </a>`
                : '<p class="no-file">No attached file.</p>'
            }
        `;
        list.appendChild(item);
    });
}
