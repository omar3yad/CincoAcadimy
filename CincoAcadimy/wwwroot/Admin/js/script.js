
        // Sample data for the dashboard
    const sampleData = {
        payments: [
    {id: 1, student: "John Smith", course: "Web Development Bootcamp", amount: 299, date: "2023-05-15", status: "completed" },
    {id: 2, student: "Emma Johnson", course: "Data Science Fundamentals", amount: 199, date: "2023-05-14", status: "completed" },
    {id: 3, student: "Michael Brown", course: "Digital Marketing Mastery", amount: 149, date: "2023-05-13", status: "pending" },
    {id: 4, student: "Sarah Davis", course: "UX/UI Design Principles", amount: 179, date: "2023-05-12", status: "completed" },
    {id: 5, student: "Robert Wilson", course: "Business Analytics", amount: 249, date: "2023-05-11", status: "failed" },
    {id: 6, student: "Lisa Anderson", course: "Python for Beginners", amount: 99, date: "2023-05-10", status: "completed" },
    {id: 7, student: "James Miller", course: "Advanced JavaScript", amount: 199, date: "2023-05-09", status: "completed" },
    {id: 8, student: "Jennifer Taylor", course: "Mobile App Development", amount: 349, date: "2023-05-08", status: "pending" }
    ],
    courses: [
    {id: 1, name: "Web Development Bootcamp", category: "web-development", students: 342, price: 299, status: "active" },
    {id: 2, name: "Data Science Fundamentals", category: "data-science", students: 215, price: 199, status: "active" },
    {id: 3, name: "Digital Marketing Mastery", category: "digital-marketing", students: 187, price: 149, status: "active" },
    {id: 4, name: "UX/UI Design Principles", category: "design", students: 156, price: 179, status: "active" },
    {id: 5, name: "Business Analytics", category: "business", students: 124, price: 249, status: "draft" },
    {id: 6, name: "Python for Beginners", category: "web-development", students: 298, price: 99, status: "active" },
    {id: 7, name: "Advanced JavaScript", category: "web-development", students: 176, price: 199, status: "active" },
    {id: 8, name: "Mobile App Development", category: "web-development", students: 203, price: 349, status: "active" }
    ],
    hrStaff: [
    {id: 1, name: "David Wilson", email: "david.wilson@lms.com", department: "recruitment", role: "manager", status: "active" },
    {id: 2, name: "Emily Clark", email: "emily.clark@lms.com", department: "training", role: "specialist", status: "active" },
    {id: 3, name: "Brian Roberts", email: "brian.roberts@lms.com", department: "operations", role: "coordinator", status: "active" },
    {id: 4, name: "Amanda Lee", email: "amanda.lee@lms.com", department: "support", role: "assistant", status: "inactive" },
    {id: 5, name: "Kevin Martin", email: "kevin.martin@lms.com", department: "recruitment", role: "specialist", status: "active" }
    ],
    trainers: [
    {id: 1, name: "Dr. Sarah Johnson", specialization: "web-development", courses: 8, rating: 4.9, status: "active" },
    {id: 2, name: "Prof. Michael Chen", specialization: "data-science", courses: 5, rating: 4.8, status: "active" },
    {id: 3, name: "Alex Rodriguez", specialization: "digital-marketing", courses: 6, rating: 4.7, status: "active" },
    {id: 4, name: "Lisa Thompson", specialization: "design", courses: 4, rating: 4.9, status: "active" },
    {id: 5, name: "James Wilson", specialization: "business", courses: 3, rating: 4.6, status: "inactive" },
    {id: 6, name: "Rachel Green", specialization: "web-development", courses: 7, rating: 4.8, status: "active" }
    ]
        };

    // DOM Elements
    const menuItems = document.querySelectorAll('.menu-item');
    const contentSections = document.querySelectorAll('.content-section');
    const tabs = document.querySelectorAll('.tab');
    const tabContents = document.querySelectorAll('.tab-content');

    // Initialize the dashboard
    document.addEventListener('DOMContentLoaded', function () {
        // Load initial data
        loadPaymentsTable();
    loadAllPaymentsTable();
    loadCoursesTable();
    loadHRTable();
    loadTrainersTable();
    generateRevenueChart();

    // Set up event listeners
    setupEventListeners();
        });

    // Set up all event listeners
    function setupEventListeners() {
        // Sidebar navigation
        menuItems.forEach(item => {
            item.addEventListener('click', function (e) {
                e.preventDefault();
                const targetId = this.getAttribute('data-target');

                // Update active menu item
                menuItems.forEach(i => i.classList.remove('active'));
                this.classList.add('active');

                // Show target section
                contentSections.forEach(section => {
                    section.classList.remove('active');
                    if (section.id === targetId) {
                        section.classList.add('active');
                    }
                });
            });
        });

            // Tab navigation
            tabs.forEach(tab => {
        tab.addEventListener('click', function () {
            const tabId = this.getAttribute('data-tab');

            // Update active tab
            tabs.forEach(t => t.classList.remove('active'));
            this.classList.add('active');

            // Show target tab content
            tabContents.forEach(content => {
                content.classList.remove('active');
                if (content.id === tabId) {
                    content.classList.add('active');
                }
            });
        });
            });

    // Add buttons for switching to add forms
    document.getElementById('add-course-btn').addEventListener('click', function () {
        tabs.forEach(t => t.classList.remove('active'));
    document.querySelector('[data-tab="add-course"]').classList.add('active');

                tabContents.forEach(content => {
        content.classList.remove('active');
    if (content.id === 'add-course') {
        content.classList.add('active');
                    }
                });
            });

    document.getElementById('add-hr-btn').addEventListener('click', function () {
        tabs.forEach(t => t.classList.remove('active'));
    document.querySelector('[data-tab="add-hr"]').classList.add('active');

                tabContents.forEach(content => {
        content.classList.remove('active');
    if (content.id === 'add-hr') {
        content.classList.add('active');
                    }
                });
            });

    document.getElementById('add-trainer-btn').addEventListener('click', function () {
        tabs.forEach(t => t.classList.remove('active'));
    document.querySelector('[data-tab="add-trainer"]').classList.add('active');

                tabContents.forEach(content => {
        content.classList.remove('active');
    if (content.id === 'add-trainer') {
        content.classList.add('active');
                    }
                });
            });

    // Form submissions
    document.getElementById('course-form').addEventListener('submit', function (e) {
        e.preventDefault();
    alert('Course added successfully!');
    this.reset();

                // Switch back to courses list
                tabs.forEach(t => t.classList.remove('active'));
    document.querySelector('[data-tab="courses-list"]').classList.add('active');

                tabContents.forEach(content => {
        content.classList.remove('active');
    if (content.id === 'courses-list') {
        content.classList.add('active');
                    }
                });

    // Reload courses table
    loadCoursesTable();
            });

    document.getElementById('hr-form').addEventListener('submit', function (e) {
        e.preventDefault();
    alert('HR staff added successfully!');
    this.reset();

                // Switch back to HR list
                tabs.forEach(t => t.classList.remove('active'));
    document.querySelector('[data-tab="hr-list"]').classList.add('active');

                tabContents.forEach(content => {
        content.classList.remove('active');
    if (content.id === 'hr-list') {
        content.classList.add('active');
                    }
                });

    // Reload HR table
    loadHRTable();
            });

    document.getElementById('trainer-form').addEventListener('submit', function (e) {
        e.preventDefault();
    alert('Trainer added successfully!');
    this.reset();

                // Switch back to trainers list
                tabs.forEach(t => t.classList.remove('active'));
    document.querySelector('[data-tab="trainers-list"]').classList.add('active');

                tabContents.forEach(content => {
        content.classList.remove('active');
    if (content.id === 'trainers-list') {
        content.classList.add('active');
                    }
                });

    // Reload trainers table
    loadTrainersTable();
            });

    document.getElementById('settings-form').addEventListener('submit', function (e) {
        e.preventDefault();
    alert('Settings saved successfully!');
            });
        }

    // Load payments table for dashboard
    function loadPaymentsTable() {
            const tableBody = document.getElementById('payments-table');
    tableBody.innerHTML = '';

    // Show only recent 5 payments
    const recentPayments = sampleData.payments.slice(0, 5);

            recentPayments.forEach(payment => {
                const row = document.createElement('tr');
    row.innerHTML = `
    <td>${payment.student}</td>
    <td>${payment.course}</td>
    <td>$${payment.amount}</td>
    <td>${payment.date}</td>
    <td><span class="status ${payment.status}">${payment.status}</span></td>
    `;
    tableBody.appendChild(row);
            });
        }

    // Load all payments table
    function loadAllPaymentsTable() {
            const tableBody = document.getElementById('all-payments-table');
    tableBody.innerHTML = '';

            sampleData.payments.forEach(payment => {
                const row = document.createElement('tr');
    row.innerHTML = `
    <td>#${payment.id}</td>
    <td>${payment.student}</td>
    <td>${payment.course}</td>
    <td>$${payment.amount}</td>
    <td>${payment.date}</td>
    <td><span class="status ${payment.status}">${payment.status}</span></td>
    <td>
        <button class="btn btn-primary" style="padding: 5px 10px; font-size: 0.8rem;">View</button>
        <button class="btn btn-danger" style="padding: 5px 10px; font-size: 0.8rem;">Delete</button>
    </td>
    `;
    tableBody.appendChild(row);
            });
        }

    // Load courses table
    function loadCoursesTable() {
            const tableBody = document.getElementById('courses-table');
    tableBody.innerHTML = '';

            sampleData.courses.forEach(course => {
                const row = document.createElement('tr');
    row.innerHTML = `
    <td>#${course.id}</td>
    <td>${course.name}</td>
    <td>${course.category}</td>
    <td>${course.students}</td>
    <td>$${course.price}</td>
    <td><span class="status ${course.status}">${course.status}</span></td>
    <td>
        <div class="action-buttons">
            <button class="btn btn-primary" style="padding: 5px 10px; font-size: 0.8rem;">Edit</button>
            <button class="btn btn-danger" style="padding: 5px 10px; font-size: 0.8rem;">Delete</button>
        </div>
    </td>
    `;
    tableBody.appendChild(row);
            });
        }

    // Load HR table
    function loadHRTable() {
            const tableBody = document.getElementById('hr-table');
    tableBody.innerHTML = '';

            sampleData.hrStaff.forEach(staff => {
                const row = document.createElement('tr');
    row.innerHTML = `
    <td>#${staff.id}</td>
    <td>${staff.name}</td>
    <td>${staff.email}</td>
    <td>${staff.department}</td>
    <td>${staff.role}</td>
    <td><span class="status ${staff.status}">${staff.status}</span></td>
    <td>
        <div class="action-buttons">
            <button class="btn btn-primary" style="padding: 5px 10px; font-size: 0.8rem;">Edit</button>
            <button class="btn btn-danger" style="padding: 5px 10px; font-size: 0.8rem;">Delete</button>
        </div>
    </td>
    `;
    tableBody.appendChild(row);
            });
        }

    // Load trainers table
    function loadTrainersTable() {
            const tableBody = document.getElementById('trainers-table');
    tableBody.innerHTML = '';

            sampleData.trainers.forEach(trainer => {
                const row = document.createElement('tr');
    row.innerHTML = `
    <td>#${trainer.id}</td>
    <td>${trainer.name}</td>
    <td>${trainer.specialization}</td>
    <td>${trainer.courses}</td>
    <td>${trainer.rating}/5.0</td>
    <td><span class="status ${trainer.status}">${trainer.status}</span></td>
    <td>
        <div class="action-buttons">
            <button class="btn btn-primary" style="padding: 5px 10px; font-size: 0.8rem;">Edit</button>
            <button class="btn btn-danger" style="padding: 5px 10px; font-size: 0.8rem;">Delete</button>
        </div>
    </td>
    `;
    tableBody.appendChild(row);
            });
        }

    // Generate revenue chart
    function generateRevenueChart() {
            const chartContainer = document.getElementById('revenue-chart');
    chartContainer.innerHTML = '';

    // Sample revenue data for the last 7 days
    const revenueData = [4200, 5300, 4800, 6200, 7100, 5800, 6500];
    const days = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];

    // Find the maximum value for scaling
    const maxRevenue = Math.max(...revenueData);

            revenueData.forEach((revenue, index) => {
                const barHeight = (revenue / maxRevenue) * 100;

    const bar = document.createElement('div');
    bar.className = 'chart-bar';
    bar.style.height = `${barHeight}%`;

    const label = document.createElement('div');
    label.className = 'chart-label';
    label.textContent = days[index];

    const tooltip = document.createElement('div');
    tooltip.className = 'chart-tooltip';
    tooltip.textContent = `$${revenue}`;
    tooltip.style.position = 'absolute';
    tooltip.style.top = '-30px';
    tooltip.style.left = '50%';
    tooltip.style.transform = 'translateX(-50%)';
    tooltip.style.background = 'var(--dark)';
    tooltip.style.color = 'white';
    tooltip.style.padding = '5px 10px';
    tooltip.style.borderRadius = '4px';
    tooltip.style.fontSize = '0.8rem';
    tooltip.style.opacity = '0';
    tooltip.style.transition = 'opacity 0.3s';

    bar.appendChild(label);
    bar.appendChild(tooltip);

    // Add hover effects
    bar.addEventListener('mouseover', function () {
        tooltip.style.opacity = '1';
                });

    bar.addEventListener('mouseout', function () {
        tooltip.style.opacity = '0';
                });

    chartContainer.appendChild(bar);
            });
        }