#type vertex

#version 330 core

layout (location = 0) in vec3 a_Pos;
layout (location = 1) in vec2 a_TexCoords;
uniform mat4 u_Projection;
uniform mat4 u_Model;
uniform mat4 u_View;

out vec2 v_TexCoords;

void main()
{
    gl_Position = u_Projection * u_View * u_Model * vec4(a_Pos, 1.0);
    v_TexCoords = a_TexCoords;
}

#type fragment

#version 330 core

uniform vec4 u_Color;
uniform sampler2D u_Texture;

in vec2 v_TexCoords;
out vec4 out_Color;

void main()
{
    out_Color = texture(u_Texture, v_TexCoords) * u_Color;
}