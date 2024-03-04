#version 330

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNorm;
out vec3 Normals;
out vec3 FragPos; 
uniform float angle;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;
uniform mat4 local;

mat4 rotationMatrix(float angl, vec3 axis)
{
    
    axis = normalize(axis);

    
    float s = sin(angl);
    float c = cos(angl);

    
    mat4 rot;
    rot[0][0] = c + axis.x * axis.x * (1 - c);
    rot[0][1] = axis.x * axis.y * (1 - c) - axis.z * s;
    rot[0][2] = axis.x * axis.z * (1 - c) + axis.y * s;
    rot[0][3] = 0.0f;
    rot[1][0] = axis.y * axis.x * (1 - c) + axis.z * s;
    rot[1][1] = c + axis.y * axis.y * (1 - c);
    rot[1][2] = axis.y * axis.z * (1 - c) - axis.x * s;
    rot[1][3] = 0.0f;
    rot[2][0] = axis.z * axis.x * (1 - c) - axis.y * s;
    rot[2][1] = axis.z * axis.y * (1 - c) + axis.x * s;
    rot[2][2] = c + axis.z * axis.z * (1 - c);
    rot[2][3] = 0.0f;
    rot[3][0] = 0.0f;
    rot[3][1] = 0.0f;
    rot[3][2] = 0.0f;
    rot[3][3] = 1.0f;

    return rot;
}
void main()
{
    vec3 axis = vec3(0.0, 1.0, 0.0);
    float angl = angle;
    gl_Position =  projection* view * model * local  * vec4(aPos, 1.0);
    Normals = aNorm;
    FragPos = vec3(model * vec4(aPos, 1.0f));
}
