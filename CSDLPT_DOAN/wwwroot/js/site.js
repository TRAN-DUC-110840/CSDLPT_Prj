// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Cấu hình API Base URL
window.API_BASE_URL = window.API_BASE_URL || 'http://localhost:5057/api';

// Hàm hỗ trợ xác định request đến API
function isApiRequest(input) {
    if (!input) return false;
    if (typeof input === 'string') {
        return input.includes('/api/');
    }
    if (input instanceof Request) {
        return input.url.includes('/api/');
    }
    return false;
}

// Interceptor để tự động thêm token vào các request
(function() {
    const originalFetch = window.fetch;
    
    window.fetch = function(...args) {
        const token = localStorage.getItem('authToken');
        const requestIsApi = isApiRequest(args[0]);
        
        // Nếu có token và là request đến API
        if (token && requestIsApi) {
            const options = args[1] || {};
            options.headers = options.headers || {};
            
            // Thêm Authorization header
            if (options.headers instanceof Headers) {
                options.headers.set('Authorization', `Bearer ${token}`);
            } else {
                options.headers['Authorization'] = `Bearer ${token}`;
            }
            
            args[1] = options;
        }
        
        return originalFetch.apply(this, args).then(response => {
            // Chỉ clear token nếu API trả về 401
            if (response.status === 401 && response.url && response.url.includes('/api/')) {
                localStorage.removeItem('authToken');
                localStorage.removeItem('userInfo');
                window.location.href = '/Auth/Login';
            }
            return response;
        });
    };
})();

// Hàm helper để gọi API với token tự động
async function apiCall(url, options = {}) {
    const token = localStorage.getItem('authToken');
    
    const defaultOptions = {
        headers: {
            'Content-Type': 'application/json',
            ...(token && { 'Authorization': `Bearer ${token}` })
        }
    };
    
    const mergedOptions = {
        ...defaultOptions,
        ...options,
        headers: {
            ...defaultOptions.headers,
            ...(options.headers || {})
        }
    };
    
    try {
        const response = await fetch(url, mergedOptions);
        
        if (response.status === 401) {
            localStorage.removeItem('authToken');
            localStorage.removeItem('userInfo');
            window.location.href = '/Auth/Login';
            throw new Error('Unauthorized');
        }
        
        return response;
    } catch (error) {
        console.error('API call error:', error);
        throw error;
    }
}

// Hàm đăng xuất
function logout() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userInfo');
    window.location.href = '/Auth/Login';
}

// Kiểm tra đăng nhập khi trang load
document.addEventListener('DOMContentLoaded', function() {
    const token = localStorage.getItem('authToken');
    const currentPath = window.location.pathname;
    
    // Nếu chưa đăng nhập và không phải trang login, chuyển đến trang login
    if (!token && !currentPath.includes('/Auth/Login') && !currentPath.includes('/Home/Index')) {
        // Cho phép truy cập trang chủ mà không cần đăng nhập
        // Nếu muốn bắt buộc đăng nhập, bỏ comment dòng dưới
        // window.location.href = '/Auth/Login';
    }
});