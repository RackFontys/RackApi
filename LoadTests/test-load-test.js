import http from 'k6/http';
import { sleep, check } from 'k6';

export let options = {
    stages: [
        { duration: '30s', target: 1000 }, // Ramp-up to 1000 users over 30 seconds
        { duration: '1m', target: 1000 },  // Stay at 1000 users for 5 minutes
        { duration: '10s', target: 0 },  // Ramp-down to 0 users over 10 seconds
    ],
};

export default function () {
    let res = http.get('http://localhost:5283/User?email=test&password=test');
    check(res, {
        'is status 200': (r) => r.status === 200,
    });
    sleep(1);
}
