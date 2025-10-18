// HR Management JavaScript

// API Base URL
const API_BASE_URL = 'https://localhost:44380/api/Account/by-role/Student';

// Global variables
let hrStaff = [];
let filteredStaff = [];
let editingId = null;

// DOM Elements
const hrTableBody = document.getElementById('hr-table-body');
const searchInput = document.getElementById('search-input');
const addHrBtn = document.getElementById('add-hr-btn');
const hrModal = document.getElementById('hr-modal');
const hrForm = document.getElementById('hr-form');
const modalTitle = document.getElementById('modal-title');
const hrIdInput = document.getElementById('hr-id');
const cancelBtn = document.getElementById('cancel-btn');
const closeModalBtn = document.querySelector('.close');
const loadingIndicator = document.getElementById('loading-indicator');
const noResults = document.getElementById('no-results');
const notificationContainer = document.getElementById('notification-container');

// Initialize the page
document.addEventListener('DOMContentLoaded', function () {
    loadHRStaff();
    setupEventListeners();
});

// Set up event listeners
function setupEventListeners() {
    // Search functionality
    searchInput.addEventListener('input', handleSearch);

    // Modal functionality
    addHrBtn.addEventListener('click', openAddModal);
    closeModalBtn.addEventListener('click', closeModal);
    cancelBtn.addEventListener('click', closeModal);

    // Close modal when clicking outside
    window.addEventListener('click', function (event) {
        if (event.target === hrModal) {
            closeModal();
        }
    });

    // Form submission
    hrForm.addEventListener('submit', handleFormSubmit);
}

// Load HR staff from API
async function loadHRStaff() {
    try {
        showLoading(true);
        const response = await fetch(API_BASE_URL);

        if (!response.ok) {
            throw new Error(`Failed to fetch HR staff: ${response.status}`);
        }

        hrStaff = await response.json();
        filteredStaff = [...hrStaff];
        renderHRTable();
        showLoading(false);
    } catch (error) {
        console.error('Error loading HR staff:', error);
        showNotification('Failed to load HR staff data', 'error');
        showLoading(false);
    }
}

// Render HR table
function renderHRTable() {
    if (filteredStaff.length === 0) {
        hrTableBody.innerHTML = '';
        noResults.style.display = 'flex';
        return;
    }

    noResults.style.display = 'none';

    const tableRows = filteredStaff.map(staff => `
        <tr>
            <td>${staff.name}</td>
            <td>${staff.email}</td>
            <td>${staff.phone || 'N/A'}</td>
            <td>${staff.department}</td>
            <td>${staff.position}</td>
            <td><span class="status-badge ${staff.status === 'Active' ? 'status-active' : 'status-inactive'}">${staff.status}</span></td>
            <td>
                <div class="action-buttons">
                    <button class="action-btn edit" onclick="editHRStaff(${staff.id})">
                        <i class="fas fa-edit"></i> Edit
                    </button>
                    <button class="action-btn delete" onclick="deleteHRStaff(${staff.id})">
                        <i class="fas fa-trash"></i> Delete
                    </button>
                </div>
            </td>
        </tr>
    `).join('');

    hrTableBody.innerHTML = tableRows;
}

// Handle search functionality
function handleSearch() {
    const searchTerm = searchInput.value.toLowerCase().trim();

    if (searchTerm === '') {
        filteredStaff = [...hrStaff];
    } else {
        filteredStaff = hrStaff.filter(staff =>
            staff.name.toLowerCase().includes(searchTerm) ||
            staff.department.toLowerCase().includes(searchTerm)
        );
    }

    renderHRTable();
}

// Open modal for adding new HR staff
function openAddModal() {
    editingId = null;
    modalTitle.textContent = 'Add HR Staff';
    hrForm.reset();
    hrIdInput.value = '';
    hrModal.style.display = 'block';
}

// Open modal for editing HR staff
function editHRStaff(id) {
    const staff = hrStaff.find(s => s.id === id);
    if (!staff) {
        showNotification('HR staff not found', 'error');
        return;
    }

    editingId = id;
    modalTitle.textContent = 'Edit HR Staff';

    // Fill form with staff data
    hrIdInput.value = staff.id;
    document.getElementById('name').value = staff.name;
    document.getElementById('email').value = staff.email;
    document.getElementById('phone').value = staff.phone || '';
    document.getElementById('department').value = staff.department;
    document.getElementById('position').value = staff.position;
    document.getElementById('status').value = staff.status;

    hrModal.style.display = 'block';
}

// Close modal
function closeModal() {
    hrModal.style.display = 'none';
    editingId = null;
}

// Handle form submission
async function handleFormSubmit(e) {
    e.preventDefault();

    const formData = {
        name: document.getElementById('name').value.trim(),
        email: document.getElementById('email').value.trim(),
        phone: document.getElementById('phone').value.trim(),
        department: document.getElementById('department').value,
        position: document.getElementById('position').value.trim(),
        status: document.getElementById('status').value
    };

    // Basic validation
    if (!formData.name || !formData.email || !formData.department || !formData.position) {
        showNotification('Please fill in all required fields', 'warning');
        return;
    }

    try {
        if (editingId) {
            // Update existing staff
            await updateHRStaff(editingId, formData);
        } else {
            // Add new staff
            await addHRStaff(formData);
        }

        closeModal();
        await loadHRStaff(); // Reload data
    } catch (error) {
        console.error('Error saving HR staff:', error);
        showNotification('Failed to save HR staff data', 'error');
    }
}

// Add new HR staff
async function addHRStaff(staffData) {
    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(staffData)
    });

    if (!response.ok) {
        throw new Error(`Failed to add HR staff: ${response.status}`);
    }

    showNotification('HR staff added successfully', 'success');
}

// Update existing HR staff
async function updateHRStaff(id, staffData) {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(staffData)
    });

    if (!response.ok) {
        throw new Error(`Failed to update HR staff: ${response.status}`);
    }

    showNotification('HR staff updated successfully', 'success');
}

// Delete HR staff
async function deleteHRStaff(id) {
    if (!confirm('Are you sure you want to delete this HR staff member?')) {
        return;
    }

    try {
        const response = await fetch(`${API_BASE_URL}/${id}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            throw new Error(`Failed to delete HR staff: ${response.status}`);
        }

        showNotification('HR staff deleted successfully', 'success');
        await loadHRStaff(); // Reload data
    } catch (error) {
        console.error('Error deleting HR staff:', error);
        showNotification('Failed to delete HR staff', 'error');
    }
}

// Show/hide loading indicator
function showLoading(show) {
    loadingIndicator.style.display = show ? 'flex' : 'none';
}

// Show notification
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `notification ${type}`;
    notification.innerHTML = `
        <i class="fas fa-${getNotificationIcon(type)}"></i>
        <span>${message}</span>
        <span class="close-notification">&times;</span>
    `;

    notificationContainer.appendChild(notification);

    // Auto remove after 5 seconds
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, 5000);

    // Close on click
    notification.querySelector('.close-notification').addEventListener('click', () => {
        notification.remove();
    });
}

// Get appropriate icon for notification type
function getNotificationIcon(type) {
    switch (type) {
        case 'success': return 'check-circle';
        case 'error': return 'exclamation-circle';
        case 'warning': return 'exclamation-triangle';
        default: return 'info-circle';
    }
}